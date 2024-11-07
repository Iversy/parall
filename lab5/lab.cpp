#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <unordered_map>
#include <omp.h>
#include <vector>

using namespace std;
double processLogFileSingle(const string &filename)
{
    double time = omp_get_wtime();
    ifstream logFile(filename);
    if (!logFile.is_open())
    {
        cerr << "Ошибка открытия файла: " << filename << endl;
        return 0;
    }

    string line;
    unordered_map<string, int> methodCount = {
        {"GET", 0},
        {"POST", 0},
        {"PUT", 0},
        {"DELETE", 0},
        {"HEAD", 0}};

    while (getline(logFile, line))
    {
        istringstream iss(line);
        string ip, dash, timestamp, method;

        if (iss >> ip >> dash >> dash >> timestamp >> method)
        {
            if (methodCount.find(method) != methodCount.end())
            {
                methodCount[method]++;
            }
        }
    }

    logFile.close();

    cout << "Results:" << endl;
    int sm = 0;
    for (const auto &pair : methodCount)
    {
        cout << pair.first << ": " << pair.second << endl;
        sm += pair.second;
    }
    cout << "Всего = " << sm << endl;
    return omp_get_wtime() - time;
}

double processLogFileMulti(const string &filename)
{
    double time = omp_get_wtime();
    ifstream logFile(filename);
    if (!logFile.is_open())
    {
        cerr << "Ошибка открытия файла: " << filename << endl;
        return 0;
    }

    string line;
    unordered_map<string, int> methodCount = {
        {"GET", 0},
        {"POST", 0},
        {"PUT", 0},
        {"DELETE", 0},
        {"HEAD", 0}};

    vector<string> lines;
    while (getline(logFile, line))
    {
        lines.push_back(line);
    }
    logFile.close();

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
            istringstream iss(lines[i]);
            string ip, dash, timestamp, method;

            if (iss >> ip >> dash >> dash >> timestamp >> method)
            {
                if (localCount.find(method) != localCount.end())
                {
                    localCount[method]++;
                }
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
    int sm = 0;
    for (const auto &pair : methodCount)
    {
        cout << pair.first << ": " << pair.second << endl;
        sm += pair.second;
    }
    cout << "Всего = " << sm << endl;
    return omp_get_wtime() - time;
}

int main()
{
    string filename = "logs";
    // cout << "колво потоков:" << omp_get_num_threads() << endl;
    cout << "1 Поток " << processLogFileSingle(filename) << " сек\n";
    cout << "12 Потоков " << processLogFileMulti(filename) << " сек\n";
    return 0;
}
