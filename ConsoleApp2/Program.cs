using System.Diagnostics;

void ParallelBlockMatrixMultiply(int[,] matrix1, int[,] matrix2, int[,] resultMatrix, int[] block)
{
    int rowStart = block[0], rowEnd = block[1], colStart = block[2], colEnd = block[3];
    for (int i = rowStart; i < rowEnd; i++)
    {
        for (int j = colStart; j < colEnd; j++)
        {
            for (int l = 0; l < matrix1.GetLength(0); l++)
            {
                resultMatrix[i, j] += matrix1[i, l] * matrix2[l, j];
            }
        }
    }
}


int[,] CreateMatrix(int N, int seed)
{
    int[,] matrix = new int[N, N];
    Random rnd = new Random(seed);
    for (int i = 0; i < matrix.GetLength(0); i++)
        for (int j = 0; j < matrix.GetLength(1); j++)
            matrix[i, j] = rnd.Next(0, 10);
    return matrix;
}

void PrintMatrix(int[,] matrix)
{
    for (int i = 0; i < matrix.GetLength(0); i++)
    {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            Console.Write("{0}\t", matrix[i, j]);
        }
        Console.WriteLine();
    }
}

void ParallelRowMatrixMultiply(int[,] matrix1, int[,] matrix2, int[,] resultMatrix, int i)
{

    for (int j = 0; j < matrix2.GetLength(1); j++)
    {
        for (int l = 0; l < matrix1.GetLength(0); l++)
        {
            resultMatrix[i, j] += matrix1[i, l] * matrix2[l, j];
        }
    }

}

void ParallelColMatrixMultiply(int[,] matrix1, int[,] matrix2, int[,] resultMatrix, int j)
{

    for (int i = 0; i < matrix1.GetLength(0); i++)
    {
        for (int l = 0; l < matrix1.GetLength(0); l++)
        {
            resultMatrix[i, j] += matrix1[i, l] * matrix2[l, j];
        }
    }

}

double SeqColTest(int size, int thred)
{
    int[,] asd = CreateMatrix(size, 9);
    int[,] bsd = CreateMatrix(size, 10);
    int[,] result = new int[size, size];
    ParallelOptions options = new ParallelOptions
    {
        MaxDegreeOfParallelism = thred
    };
    Stopwatch sw = new Stopwatch();
    sw.Start();
    Parallel.For(0, size, options, j => ParallelColMatrixMultiply(asd, bsd, result, j));
    sw.Stop();
    return sw.Elapsed.TotalSeconds;
}

double SeqRowTest(int size, int thred)
{
    int[,] asd = CreateMatrix(size, 9);
    int[,] bsd = CreateMatrix(size, 10);
    int[,] result = new int[size, size];
    ParallelOptions options = new ParallelOptions
    {
        MaxDegreeOfParallelism = thred
    };
    Stopwatch sw = new Stopwatch();
    sw.Start();
    Parallel.For(0, size, options, i => ParallelRowMatrixMultiply(asd, bsd, result, i));
    sw.Stop();
    // PrintMatrix(result);
    return sw.Elapsed.TotalSeconds;
}

double BlockTest(int size, int thred)
{
    int[,] asd = CreateMatrix(size, 9);
    int[,] bsd = CreateMatrix(size, 10);
    int[,] result = new int[size, size];
    ParallelOptions options = new ParallelOptions
    {
        MaxDegreeOfParallelism = thred
    };

    List<int[]> blocks = [];
    int rows, cols;
    if (thred == 1)
    {
        rows = 1;
        cols = 1;
    }
    else if (thred < 8)
    {
        rows = 2;
        cols = thred / rows;
    }
    else
    {
        rows = thred / 4;
        cols = thred / rows;
    }
    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            int[] block = [i * size / rows, (i + 1) * size / rows, j * size / cols, (j + 1) * size / cols];
            blocks.Add(block);
        }
    }

    Stopwatch sw = new Stopwatch();
    sw.Start();
    Parallel.ForEach(blocks, options, block => ParallelBlockMatrixMultiply(asd, bsd, result, block));
    sw.Stop();
    return sw.Elapsed.TotalSeconds;
}


void ParTestRow(int[] size, int tests_number)
{

    int[] threads = [1, 2, 4, 6, 8, 12, 16, 20];
    foreach (var s in size)
    {
        foreach (var number in threads)
        {
            double AVG = 0;
            for (int i = 0; i < tests_number; i++)
            {
                AVG += SeqRowTest(s, number);
            }
            Console.Write("{0}\t", AVG / tests_number);
        }
        Console.WriteLine();

    }

}

void ParTestCol(int[] size, int tests_number)
{

    int[] threads = [1, 2, 4, 6, 8, 12, 16, 20];
    foreach (var s in size)
    {
        foreach (var number in threads)
        {
            double AVG = 0;
            for (int i = 0; i < tests_number; i++)
            {
                AVG += SeqColTest(s, number);
            }
            Console.Write("{0}\t", AVG / tests_number);
        }
        Console.WriteLine();

    }

}

void ParTestBlock(int[] size, int tests_number)
{
    int[] threads = [1, 2, 4, 6, 8, 12, 16, 20];
    foreach (var s in size)
    {
        foreach (var number in threads)
        {
            double AVG = 0;
            for (int i = 0; i < tests_number; i++)
            {
                AVG += BlockTest(s, number);
            }
            Console.Write("{0}\t", AVG / tests_number);
        }
        Console.WriteLine();

    }
}





void main()
{
    // int size = 10; //, seed = 10;
    // int[,] asd = CreateMatrix(size, 9);
    // int[,] bsd = CreateMatrix(size, 10);
    // PrintMatrix(asd);
    // Console.WriteLine();
    // PrintMatrix(bsd);
    // Console.WriteLine();

    // int[,] result = new int[size, size];
    // ParallelBlockMatrixMultiply(asd, bsd, result, 0, size / 2, 0, size / 2);
    // ParallelBlockMatrixMultiply(asd, bsd, result, size / 2, size, 0, size / 2);
    // PrintMatrix(result);
    int[] size = [500, 1000, 2000];//, 2000, 5000, 10000];
    // ParTestRow(size, 10);
    // Console.WriteLine();
    // ParTestCol(size, 10);
    ParTestBlock(size, 10);

}


main();

