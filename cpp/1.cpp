#include <iostream>
#include <chrono>
#include <vector>

using namespace std::chrono;

duration<double> first(int N, int M)
{

    int *Arr = new int[N, N];
    steady_clock::time_point t1 = steady_clock::now();
    for (int j = 0; j < N; j++)
        for (int i = 0; i < N; i++)
            Arr[j, i] = i / (j + 1);

    steady_clock::time_point t2 = steady_clock::now();
    return duration_cast<duration<double>>(t2 - t1);
}

duration<double> second(int N, int M)
{
    int *Arr = new int[N, N];
    steady_clock::time_point t11 = steady_clock::now();
    for (int i = 0; i < N; i++)
        for (int j = 0; j < N; j++)
            Arr[j, i] = i / (j + 1);

    steady_clock::time_point t21 = steady_clock::now();
    return duration_cast<duration<double>>(t21 - t11);
}

duration<double> third(int N, int M)
{
    int *Arr = new int[N, N];
    steady_clock::time_point t12 = steady_clock::now();
    for (int i = N - 1; i > 0; i--)
        for (int j = N - 1; j > 0; j--)
            Arr[i, j] = i / (j + 1);

    steady_clock::time_point t22 = steady_clock::now();
    return duration_cast<duration<double>>(t22 - t12);
}

duration<double> fourth(int N, int M)
{
    int *Arr = new int[N, N];
    steady_clock::time_point t13 = steady_clock::now();
    for (int j = N - 1; j > 0; j--)
        for (int i = N - 1; i > 0; i--)
            Arr[i, j] = i / (j + 1);

    steady_clock::time_point t23 = steady_clock::now();
    return duration_cast<duration<double>>(t23 - t13);
}

int main()
{

    // steady_clock::time_point t1 = steady_clock::now();

    // for (int i = 0; i < 10000; ++i)
    //     std::cout << '.';
    // std::cout << std::endl;
    // steady_clock::time_point t2 = steady_clock::now();
    // duration<double> time_span = duration_cast<duration<double>>(t2 - t1);

    // std::cout << time_span.count() << " seconds" << std::endl;

    int N = 6000, M = 1000;
    int *Arr = new int[N, N];

    // std::cout << time_span1.count() << " seconds" << std::endl;
    // for (int j = 0; j < N; j++)
    //     for (int i = 0; i < N; i++)
    //         std::cout << Arr[j, i] << " ";
    duration<double> minimum1 = seconds(10000), maximum1 = seconds(0), average1 = seconds(0);
    int number_repeat = 1000;
    for (int i = 0; i < number_repeat; ++i)
    {
        duration<double> current = first(N, M);
        minimum1 = std::min(current, minimum1);
        maximum1 = std::max(current, maximum1);
        average1 += current;
    }
    average1 /= number_repeat;
    std::cout << "Первый способ" << std::endl;
    std::cout << "Минимум = " << minimum1.count() << " секунд\nМаксимум = " << maximum1.count() << " секунд\nСреднее = " << average1.count() << " секунд" << std::endl;

    duration<double> minimum2 = seconds(10000), maximum2 = seconds(0), average2;
    for (int i = 0; i < number_repeat; ++i)
    {
        duration<double> current = second(N, M);
        minimum2 = std::min(current, minimum2);
        maximum2 = std::max(current, maximum2);
        average2 += current;
    }
    average2 /= number_repeat;
    std::cout << "Второй способ" << std::endl;
    std::cout << "Минимум = " << minimum2.count() << " секунд\nМаксимум = " << maximum2.count() << " секунд\nСреднее = " << average2.count() << " секунд" << std::endl;

    duration<double> minimum3 = seconds(10000), maximum3 = seconds(0), average3;
    for (int i = 0; i < number_repeat; ++i)
    {
        duration<double> current = third(N, M);
        minimum3 = std::min(current, minimum3);
        maximum3 = std::max(current, maximum3);
        average3 += current;
    }
    average3 /= number_repeat;
    std::cout << "Третий способ" << std::endl;
    std::cout << "Минимум = " << minimum3.count() << " секунд\nМаксимум = " << maximum3.count() << " секунд\nСреднее = " << average3.count() << " секунд" << std::endl;

    duration<double> minimum4 = seconds(10000), maximum4 = seconds(0), average4;
    for (int i = 0; i < number_repeat; ++i)
    {
        duration<double> current = fourth(N, M);
        minimum4 = std::min(current, minimum4);
        maximum4 = std::max(current, maximum4);
        average4 += current;
    }
    average4 /= number_repeat;
    std::cout << "Четвёртый способ" << std::endl;
    std::cout << "Минимум = " << minimum4.count() << " секунд\nМаксимум = " << maximum4.count() << " секунд\nСреднее = " << average4.count() << " секунд" << std::endl;
};
