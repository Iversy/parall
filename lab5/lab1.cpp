#include <iostream>
#include <cstdio>
#include <cstring>
#include <unordered_map>
#include <omp.h>
#include <vector>

using namespace std;

double processLogFileSingle(const string &filename)
{
    FILE *logFile = fopen(filename.c_str(), "r");
    if (!logFile)
    {
        cerr << "Ошибка открытия файла: " << filename << endl;
        return 0;
    }

    char line[256];
    unordered_map<string, int> methodCount = {
        {"GET", 0},
        {"POST", 0},
        {"PUT", 0},
        {"DELETE", 0},
        {"HEAD", 0}};
    double time = omp_get_wtime();

    while (fgets(line, sizeof(line), logFile))
    {
        char ip[16], dash[10], timestamp[30], method[10];
        if (sscanf(line, "%15s %9s %9s %29s %9s", ip, dash, dash, timestamp, method) == 5)
        {
            if (methodCount.find(method) != methodCount.end())
            {
                methodCount[method]++;
            }
        }
    }

    fclose(logFile);

    cout << "Results:" << endl;
    for (const auto &pair : methodCount)
    {
        cout << pair.first << ": " << pair.second << endl;
    }
    return omp_get_wtime() - time;
}

double processLogFileMulti(const string &filename)
{
    FILE *logFile = fopen(filename.c_str(), "r");
    if (!logFile)
    {
        cerr << "Ошибка открытия файла: " << filename << endl;
        return 0;
    }

    char line[256];
    unordered_map<string, int> methodCount = {
        {"GET", 0},
        {"POST", 0},
        {"PUT", 0},
        {"DELETE", 0},
        {"HEAD", 0}};

    vector<string> lines;
    while (fgets(line, sizeof(line), logFile))
    {
        lines.push_back(line);
    }
    fclose(logFile);

    double time = omp_get_wtime();
#pragma omp parallel num_threads(12)
    {
        unordered_map<string, int> localCount = {
            {"GET", 0},
            {"POST", 0},
            {"PUT", 0},
            {"DELETE", 0},
            {"HEAD", 0}};

#pragma omp for
        for (size_t i = 0; i < lines.size(); ++i)
        {
            const char *method = strstr(lines[i].c_str(), "GET") ? "GET" : strstr(lines[i].c_str(), "POST") ? "POST"
                                                                       : strstr(lines[i].c_str(), "PUT")    ? "PUT"
                                                                       : strstr(lines[i].c_str(), "DELETE") ? "DELETE"
                                                                       : strstr(lines[i].c_str(), "HEAD")   ? "HEAD"
                                                                                                            : nullptr;

            if (method)
            {
                localCount[method]++;
            }
        }

#pragma omp critical
        {
            for (const auto &pair : localCount)
            {
                methodCount[pair.first] += pair.second;
            }
        }
    }

    cout << "Results:" << endl;
    for (const auto &pair : methodCount)
    {
        cout << pair.first << ": " << pair.second << endl;
    }
    return omp_get_wtime() - time;
}

int main()
{
    string filename = "logs";
    cout << "1 Поток " << processLogFileSingle(filename) << " сек\n";
    cout << "12 Потоков " << processLogFileMulti(filename) << " сек\n";
    return 0;
}
