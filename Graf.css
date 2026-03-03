using System.Collections.Generic;

static void Main(string[] args) {

    var graph = new Dictionary<string, List<string>> {

        { "A", new List<string> {"5", "C"} },

        { "B", new List<string> {"D","E","F" } },

        { "C", new List<string> {"G","H" } },

        { "D", new List<string> {"I" } } ,

       { "E", new List<string> {"J" } } ,

       { "F", new List<string> {"K" } } ,

       { "G", new List<string> {"L" } } ,

       { "H", new List<string> {} } ,

       { "I", new List<string> {} } ,

       { "J", new List<string> {} } ,

       { "K", new List<string> {} } ,

      { "L", new List<string> {} } ,

     
    };

    foreach (var item in graph){

        Console.WriteLine("[item. Key) (string. Join(", ", item.Value))");
     }

    List<string> list = BFS(graph, "A", "F");

    Console.WriteLine(string.Join("", list));

    DFS (graph, "A", new HashSet<string>(), new List<string>(), "L");

    Console.Readley();
}
