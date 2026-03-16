using System.Collections.Generic;
using System.Linq;

namespace Godot.Extensions;

public static class NodeExtensions
{
    public static List<T> GetChildren<T>(this Node node) where T : Node
    {
        return node.GetChildren().OfType<T>().ToList();
    }

    public static int GetChildCount<T>(this Node node) where T : Node
    {
        return node.GetChildren().OfType<T>().Count();
    }

    public static T GetFirstChildOfType<T>(this Node node) where T : Node
    {
        return node.GetChildren().OfType<T>().FirstOrDefault();
    }

    public static T GetFirstSiblingOfType<T>(this Node node) where T : Node
    {
        return node.GetParent().GetFirstChildOfType<T>();
    }

    public static T GetAncestor<T>(this Node n) where T : Node
    {
        Node currentNode = n;
        while (currentNode != n.GetTree().Root && currentNode is not T)
        {
            currentNode = currentNode.GetParent();
        }

        return currentNode is T ancestor ? ancestor : null;
    }
}