using System;
using System.Collections.Generic;

public class Graph
{
    private int _vertexCount;
    private List<int>[] _adjacencyLists;

    public Graph(int vertexCount)
    {
        _vertexCount = vertexCount;
        _adjacencyLists = new List<int>[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            _adjacencyLists[i] = new List<int>();
        }
    }

    public void AddEdge(int source, int destination)
    {
        _adjacencyLists[source].Add(destination);
        _adjacencyLists[destination].Add(source); 
    }

  
    public List<int> GetNeighbors(int vertex)
    {
        return _adjacencyLists[vertex];
    }
    public static Graph CreateRandomGraph(int vertexCount, double edgeProbability = 0.01)
    {
        var graph = new Graph(vertexCount);
        var random = new Random();

        for (int i = 0; i < vertexCount; i++)
        {
            for (int j = i + 1; j < vertexCount; j++)
            {
                if (random.NextDouble() < edgeProbability)
                {
                    graph.AddEdge(i, j);
                }
            }
        }

        return graph;
    }
    public void DFSRecursive(int startVertex)
    {
        bool[] visited = new bool[_vertexCount];
        DFSRecursiveHelper(startVertex, visited);
    }

    private void DFSRecursiveHelper(int vertex, bool[] visited)
    {
        visited[vertex] = true;
        Console.WriteLine($"Посещён узел: {vertex}");

        foreach (int neighbor in GetNeighbors(vertex))
        {
            if (!visited[neighbor])
            {
                DFSRecursiveHelper(neighbor, visited);
            }
        }
    }
    public void DFSIterative(int startVertex)
    {
        bool[] visited = new bool[_vertexCount];
        Stack<int> stack = new Stack<int>();

        stack.Push(startVertex);

        while (stack.Count > 0)
        {
            int currentVertex = stack.Pop();

            if (!visited[currentVertex])
            {
                visited[currentVertex] = true;
                Console.WriteLine($"Посещён узел: {currentVertex}");


                foreach (int neighbor in GetNeighbors(currentVertex))
                {
                    if (!visited[neighbor])
                    {
                        stack.Push(neighbor);
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            const int VERTEX_COUNT = 1000;
            Graph graph = CreateRandomGraph(VERTEX_COUNT, 0.01);

            Console.WriteLine("Начинаем обход в глубину...");

            graph.DFSRecursive(0);


        }
    }
}
