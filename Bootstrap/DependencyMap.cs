using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Unmoddable;

/// <summary>
/// Map of dependencies. Resolved using Kahn's Algorithm.
/// </summary>
public sealed class DependencyMap<TKey, TValue>(Func<TValue, ICollection<TKey>> dependencyProvider, EqualityComparer<TKey> comparer)
    : IEnumerable<TValue>/*, IDictionary<TKey, T>, ICollection<T>, IDisposable*/
{
    /// <summary>
    /// Direct accessor to an internal <see cref="Dictionary{TKey, TValue}"/> with all <see cref="TValue"/>s.
    /// </summary>
    public TValue this[TKey key]
    {
        get => Map[key];
        set
        {
            Map[key] = value;
            Sources = null;
        }
    }

    /// <inheritdoc/>
    public int Count => Map.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                                Constructors
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public DependencyMap(Func<TValue, ICollection<TKey>> dependencyProvider)
        : this(dependencyProvider, EqualityComparer<TKey>.Default) { }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Private Fields
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    /// <summary>
    /// Map of all dependencies.
    /// </summary>
    private readonly Dictionary<TKey, TValue> Map = [with(comparer ?? EqualityComparer<TKey>.Default)];
    /// <summary>
    /// Provides dependency requirements for each node.
    /// </summary>
    private readonly Func<TValue, ICollection<TKey>> DependencyProvider = dependencyProvider;
    /// <summary>
    /// Ordered sources.
    /// </summary>
    private IReadOnlyList<TValue>? Sources = [];




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                               Public Methods
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    private sealed class Node(TKey key, TValue value, ICollection<TKey> dependencies)
    {
        public readonly TKey Key = key;
        public readonly TValue Value = value;
        public readonly ICollection<TKey> Dependencies = dependencies;
        public readonly HashSet<Node> Children = [];
        public int InDegree;
        public override string ToString() => $"{Key}";
    }

    /// <summary>
    /// Attempts to resolve all dependencies. Returns cached <paramref name="sources"/> array if already resolved.
    /// </summary>
    /// <returns>
    /// <c>true</c> if all dependencies were resolved successfully. <c>false</c> if dependencies are missing or incompatible.
    /// </returns>
    public bool TryResolve(out IReadOnlyList<TValue> sources)
    {
        // Cached result is returned by default.
        if (Sources is not null)
        {
            sources = Sources;
            return true;
        }

        //bool ExpectFrequentReloading = false;
        //if (ExpectFrequentReloading)
        //{
        //    // TODO: Use cached Queue and Arrays.
        //    throw new NotSupportedException("Frequent reloading is not supported.");
        //}

        // Lists all nodes.
        int index = 0;
        Node[] nodes = new Node[Count];
        Dictionary<TKey, Node> map = new(nodes.Length, Map.Comparer);
        foreach (var pair in Map)
        {
            Node node = new(pair.Key, pair.Value, DependencyProvider(pair.Value));
            nodes[index++] = node;
            map[pair.Key] = node;
        }

        // Creates associations.
        for (int i = 0; i < nodes.Length; i++)
        {
            Node node = nodes[i];
            foreach (var dependency in node.Dependencies)
            {
                if (map.TryGetValue(dependency, out Node target))
                {
                    node.InDegree++;
                    target.Children.Add(node);
                }
            }
        }

        // Resolves all dependencies.
        TValue[] result = new TValue[nodes.Length];
        Queue<Node> queue = new(nodes.Length);
        for (int i = 0; i < nodes.Length; i++)
        {
            // Should start from a root.
            if (nodes[i].InDegree == 0)
                queue.Enqueue(nodes[i]);
        }

        int head = 0;
        while (queue.TryDequeue(out Node node))
        {
            foreach (var child in node.Children)
            {
                if (--child.InDegree == 0)
                    queue.Enqueue(child);
            }

            result[head++] = node.Value;
        }

        if (head == result.Length)
        {
            sources = result;
            return true;
        }

        // Some nodes are in a loop.
        sources = [];
        return false;
    }




    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===<![CDATA[
    /// .
    /// .                                              Implementations
    /// .
    /// ===     ===     ===     ===    ===  == =  -                        -  = ==  ===    ===     ===     ===     ===]]>
    public override string ToString()
    {
        StringBuilder builder = new();
        builder.AppendLine($"Dependency map (length: {Map.Count}). Is resolved? ({Sources is not null})");
        if (Sources is not null)
        {
            int i = 0;
            foreach (var item in Sources)
            {
                builder.AppendLine($"[{i++}] {item}");
            }
        }
        else
        {
            int i = 0;
            foreach (var dependency in Map)
            {
                builder.AppendLine($"[{i++}] {dependency}");
            }
        }

        return builder.ToString();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public IEnumerator<TValue> GetEnumerator() => Map.Values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => Map.Values.GetEnumerator();

    /// <inheritdoc cref="Add(TKey, TValue)"/>
    public void Add(TKey key, TValue value) => Map.Add(key, value);

    /// <inheritdoc cref="Add(TKey, TValue)"/>
    public bool TryAdd(TKey key, TValue value) => Map.TryAdd(key, value);

    /// <inheritdoc/>
    public void Clear()
    {
        Map.Clear();
        Sources = [];
    }

    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => Map.ContainsKey(key);

    /// <inheritdoc/>
    public bool Remove(TKey key) => Map.Remove(key);

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value) => Map.TryGetValue(key, out value);

    /// <inheritdoc/>
    public void Dispose()
    {
        Map.Clear();
        Sources = null;
    }
}