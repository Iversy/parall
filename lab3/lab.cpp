#include <iostream>
#include <omp.h>
#include <vector>

using namespace std;

void task_A(int N)
{
    int sum = 0;
#pragma omp parallel num_threads(2) reduction(+ : sum)
    {

        int thread_id = omp_get_thread_num();
        for (int i = N / 2 * thread_id + 1; i <= N / 2 * (thread_id + 1); ++i)
        {
            sum += i;
        }

#pragma omp critical
        {
            cout << "[" << omp_get_thread_num() << "]: Sum = " << sum << "\n";
        }
    }
    cout << "Sum = " << sum << endl;
}

void task_B(int N, int thread_number)
{
    int sum = 0;
#pragma omp parallel num_threads(thread_number) reduction(+ : sum)
    {
        //
        int thread_id = omp_get_thread_num();
        int start = N / thread_number * thread_id + 1;
        int end = (thread_id + 1) == thread_number ? N : N / thread_number * (thread_id + 1);
        cout << start << " " << end << " T = " << thread_id << "\n";
        for (int i = start; i <= end; ++i)
        {
            sum += i;
        }
#pragma omp critical
        {
            cout << "[" << omp_get_thread_num() << "]: Sum = " << sum << "\n";
        }
    }
    cout << "Sum = " << sum << endl;
}

void task_C(int N, int thread_number)
{
    int sum = 0;

#pragma omp parallel reduction(+ : sum) num_threads(thread_number)
    {
#pragma omp for
        for (int i = 1; i <= N; ++i)
        {
            sum += i;
        }

#pragma omp critical
        {
            cout << "[" << omp_get_thread_num() << "]: Sum = " << sum << "\n";
        }
    }

    cout << "Sum = " << sum << endl;
}

void task_D(int N, int thread_number)
{
    int sum = 0;
#pragma omp parallel reduction(+ : sum) num_threads(thread_number)
    {
#pragma omp for schedule(static)
        for (int i = 1; i <= N; ++i)
        {
            sum += i;
            cout << format("[{}]: calculation of the iteration number {}\n", omp_get_thread_num(), i);
        }
#pragma omp critical
        {
            cout << "[" << omp_get_thread_num() << "]: Sum = " << sum << "\n";
        }
    }

    cout << "Sum = " << sum << endl;
}

int main() // int argc, char *argv[])
{
    // omp_set_num_threads(6);

    int n = 10, k = 4;
    // if (argc == 0)
    // {
    //     return 0;
    // }
    // n = argc ? (int)argv[0] : 10;
    // k = argc > 1 ? (int)argv[1] : 6;

    // task_A(n);
    // task_B(n, k);
    // task_C(n, k);
    task_D(n, k);
}