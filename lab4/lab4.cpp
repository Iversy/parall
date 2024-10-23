#include <format>
#include <iostream>
#include <omp.h>
#include <random>
#include <string>
#include <vector>

using namespace std;

void bubble_sort(int *a, int n)
{
    for (int i = n - 1; i > 1 - 1; i--)
        for (int j = 0; j <= i; j++)
            if (a[j] > a[j + 1])
                swap(a[j], a[j + 1]);
}

int *gen_arr(int n, int seed)
{

    // random_device rd;
    mt19937 gen(seed);
    uniform_int_distribution<> dis(-100, 100);
    int *a = new int[n];
    for (int i = 0; i < n; i++)
        a[i] = dis(gen);
    return a;
}
void print(int *arr, int n)
{
    for (int i = 0; i < n; i++)
        cout << arr[i] << " ";
    cout << endl;
}

void par_bubble_sort(int *a, int n, int threads)
{
#pragma omp parallel num_threads(threads)
    {
        for (int i = n - 1; i > 0; --i)
        {
#pragma omp for
            for (int j = 0; j <= n - 1; j += 2)
                if (a[j] > a[j + 1])
                    swap(a[j], a[j + 1]);
#pragma omp for
            for (int j = 1; j <= n - 1; j += 2)
                if (a[j] > a[j + 1])
                    swap(a[j], a[j + 1]);
        }
    }
}

void merge(int *a, int left, int mid, int right)
{
    int it1 = 0, it2 = 0;
    int *result = new int[right - left];

    while ((left + it1 < mid) && (mid + it2 < right))
    {
        if (a[left + it1] < a[mid + it2])
        {
            result[it1 + it2] = a[left + it1];
            it1 += 1;
        }
        else
        {
            result[it1 + it2] = a[mid + it2];
            it2 += 1;
        }
    }

    while (left + it1 < mid)
    {
        result[it1 + it2] = a[left + it1];
        it1 += 1;
    }

    while (mid + it2 < right)
    {
        result[it1 + it2] = a[mid + it2];
        it2 += 1;
    }

    for (int i = 0; i < it1 + it2; i++)
    {
        a[left + i] = result[i];
    }
}

void mergeSortIterative(int *a, int n, int threads)
{
    for (int i = 1; i < n; i *= 2)
    {
#pragma omp parallel for num_threads(threads)
        for (int j = 0; j < n - i; j += 2 * i)
        {
            merge(a, j, j + i, min(j + 2 * i, n));
        }
    }
}

int partition(int *a, int l, int r)
{
    int v = a[(l + r) / 2];
    int i = l, j = r;
    while (i <= j)
    {
        while (a[i] < v)
            i++;
        while (a[j] > v)
            j--;
        if (i >= j)
            break;
        swap(a[i++], a[j--]);
    }
    return j;
}

void quicksort(int *a, int l, int r)
{
    if (l >= r)
        return;
    int q = partition(a, l, r);

#pragma omp task
    quicksort(a, l, q);
#pragma omp task
    quicksort(a, q + 1, r);
}

void oleg_quicksort(int *a, int n, int threads)
{
#pragma omp parallel num_threads(threads)
#pragma omp single
    quicksort(a, 0, n - 1);
}

void shell_sort(int *a, int n)
{
}

void shell_par_sort(int *a, int n, int threads)
{
    for (int s = n / 2; s > 0; s /= 2)
#pragma omp parallel for num_threads(min(threads, s))
        for (int i = s; i < n; ++i)
            for (int j = i - s; j >= 0 && a[j] > a[j + s]; j -= s)
                swap(a[j], a[j + s]);
}

void beb_shell_par_sort(int *a, int n, int threads)
{
    for (int s = n / 2; s > 0; s /= 2)
#pragma omp parallel for num_threads(min(threads, s))
        for (int i = 0; i < s; i++)
            for (int j = i; j < n - s; j += s)
                if (a[j] > a[j + s])
                    swap(a[j], a[j + s]);
}

double test(void (*func)(int *a, int n, int threads), string name, int n,
            int threads)
{
    int *a = gen_arr(n, 1);
    double start = omp_get_wtime();
    func(a, n, threads);
    double end = omp_get_wtime();
    for (int i = 0; i < n - 1; i++)
    {
        if (a[i] > a[i + 1])
            throw invalid_argument(format("Errer {} {} > {}", name, a[i], a[i + 1]));
    }
    delete[] a;
    return end - start;
}

int main()
{
    int size = 1 << 16;
    cout << "size: " << size << endl;

    int threads[] = {1, 2, 4, 8, 12, 16, 20};
    for (auto thread : threads)
    {
        cout << format("#threads: {}, {}, {}, {}, {}\n", thread,
                       test(mergeSortIterative, "merge", size, thread),
                       test(oleg_quicksort, "oleg", size, thread),
                       test(shell_par_sort, "shell", size, thread),
                       test(par_bubble_sort, "bubble", size, thread)

        );
    }
    return 0;
}
