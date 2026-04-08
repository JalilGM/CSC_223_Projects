namespace New_CSC.src.Utilities.Containers
{
    public class DiGraph<T> where T : notnull
    {
        protected Dictionary<T, DLL<T>> _adjacencyList;
        public DiGraph()
        {
            _adjacencyList = new Dictionary<T, DLL<T>>();
        }
        public bool AddVertex(T vertex)
        {
            //Vertex already in our graph
            if (_adjacencyList.ContainsKey(vertex))
            {
                return false;
            }
            // Add vertex
            _adjacencyList.Add(vertex, new DLL<T>());
            return true;
        }
        public bool AddEdge(T source, T destination)
        {
            if(!_adjacencyList.ContainsKey(source) || !_adjacencyList.ContainsKey(destination))
            {
                throw new ArgumentException("Source and Destination must be in graph");
            }
            _adjacencyList[source].Insert(0,destination);
            return true;
        }
        public bool RemoveVertex(T vertex)
        {
            if (_adjacencyList.ContainsKey(vertex))
            {
                _adjacencyList.Remove(vertex);
                return true;
            }
            return false;
        }
        public bool RemoveEdge(T source, T destination)
        {
            if(!_adjacencyList.ContainsKey(source) || !_adjacencyList.ContainsKey(destination))
            {
                throw new ArgumentException("Source and Destination must be in graph");
            }
            _adjacencyList[source].Remove(destination);
            return true;
        }
        public bool HasEdge(T source, T destination)
        {
            if (_adjacencyList[
                source].Contains(destination))
            {
                return true;
            }
            return false;
        }
        public List<T> GetNeighbors(T vertex)
        {
            if (!_adjacencyList.ContainsKey(vertex))
            {
                throw new ArgumentException("Vertex not in graph");
            }
            List<T> neighbors =[];
            foreach (T node in _adjacencyList[vertex])
            {
                neighbors.Add(node);
            }
            return neighbors;
        }
        public IEnumerable<T> GetVertices()
        {
            foreach (T node in _adjacencyList.Keys)
            {
                yield return node;
            }
        }
        

    }
}