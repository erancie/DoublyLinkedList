using System;
using System.Text;

namespace DoublyLinkedList
{
    public class DoublyLinkedList<T>
    {

        // Here is the the nested Node<K> class 
        private class Node<K> : INode<K>
        {
            public K Value { get; set; }
            public Node<K> Next { get; set; }
            public Node<K> Previous { get; set; }

            public Node(K value, Node<K> previous, Node<K> next) //Node<K> constructor
            {
                Value = value;
                Previous = previous;
                Next = next;
            }

            public override string ToString()
            {
                StringBuilder s = new StringBuilder();
                s.Append("{");
                s.Append(Previous.Previous == null ? "XXX" : Previous.Value.ToString());
                s.Append("-(");
                s.Append(Value);
                s.Append(")-");
                s.Append(Next.Next == null ? "XXX" : Next.Value.ToString());
                s.Append("}");
                return s.ToString();
            }

        }

        private Node<T> Head { get; set; }
        private Node<T> Tail { get; set; }
        public int Count { get; private set; } = 0;

        public DoublyLinkedList() // constructor
        {
            Head = new Node<T>(default(T), null, null);
            Tail = new Node<T>(default(T), Head, null);
            Head.Next = Tail;
        }

        public INode<T> First
        {
            get
            {
                if (Count == 0) return null;
                else return Head.Next;
            }
        }

        public INode<T> Last
        {
            get
            {
                if (Count == 0) return null;
                else return Tail.Previous;
            }
        }
        public INode<T> Before(INode<T> node)
        {
            if (node == null) throw new NullReferenceException();
            Node<T> node_current = node as Node<T>;
            if (node_current.Previous == null ) throw new InvalidOperationException("The node referred as 'Before' is no longer in the list");
            if (node_current.Previous.Equals(Head)) return null;
            else return node_current.Previous;
        }
        public INode<T> After(INode<T> node)
        {
            if (node == null) throw new NullReferenceException();
            Node<T> node_current = node as Node<T>;
            if (node_current.Next == null) throw new InvalidOperationException("The node referred as 'After' is no longer in the list");
            if (node_current.Next.Equals(Tail)) return null;
            else return node_current.Next;
        }

        //Use this to insert values within the DLL
        private Node<T> AddBetween(T value, Node<T> previous, Node<T> next)
        {
            Node<T> node = new Node<T>(value, previous, next);
            previous.Next = node;
            next.Previous = node;
            Count++;
            return node;
        }

        public INode<T> Find(T value)
        {
            Node<T> node = Head.Next;
            while (!node.Equals(Tail))
            {
                if (node.Value.Equals(value)) return node;
                node = node.Next;
            }
            return null;
        }

        public override string ToString()
        {
            if (Count == 0) return "[]";
            StringBuilder s = new StringBuilder();
            s.Append("[");
            int k = 0;
            Node<T> node = Head.Next;
            while (!node.Equals(Tail))
            {
                s.Append(node.ToString());
                node = node.Next;
                if (k < Count - 1) s.Append(",");
                k++;
            }
            s.Append("]");
            return s.ToString();
        }

        public INode<T> AddLast(T value)
        {
            return AddBetween(value, Tail.Previous, Tail);
        }

        // TODO: Your task is to implement all the remaining methods.
        public INode<T> AddFirst(T value)
        {
            return AddBetween(value, Head, Head.Next); 
        }

        public INode<T> AddBefore(INode<T> node, T value) 
        {
            Node<T> current = node as Node<T>; //cast to node
            if (current == null) return AddFirst(value);  //check if first
            return AddBetween(value, current.Previous, current); //if not use AddBetween
        }

        public INode<T> AddAfter(INode<T> node, T value) 
        {
            Node<T> current = node as Node<T>;
            if (current == null) return AddLast(value);
            return AddBetween(value, current, current.Next); 
        }

        public void Clear()
        {
            Node<T> current = Head; //start at head
            while (current != null) //loop over until null
            {
                Node<T> temp = current; //place curr as temp node
                current = current.Next; //move current to next node
                nullify(temp); //nullify temp references
            }
            //relink empty list
            Head.Next = Tail;
            Tail.Previous = Head;
            //set count of list nodes to 0
            Count = 0;
            Console.WriteLine("Clear() Finished");
        }

        private void nullify(Node<T> node){
            node.Previous = null;
            node.Next = null;
            // node.Value = default(T);
        }

            // Check if nodes before and after node to be removed are not null,
            // if so, the node passed in by the tester is not in the DLL.
            // if (current.Previous == null || current.Next == null) throw new InvalidOperationException();
            
            // Check if the DLL class structure object references (prev & next) point back 
            // to the current node. If not, the node is not in the DLL.
            // if(current.Previous.Next != current || current.Next.Previous != current) throw new InvalidOperationException("Node doesn't exist in DLL");
        public void Remove(INode<T> node)
        {
            if (node == null) throw new ArgumentNullException();//check if node is valid
            Node<T> current = node as Node<T>; // cast to Node<T>
            // Check if node prev/next are not null and that the DLL object refernces prev and next can be used to point back to the node being removed.
            if(current.Previous == null || current.Previous.Next != current) throw new InvalidOperationException("Node not in DLL");
            if(current.Next == null || current.Next.Previous != current) throw new InvalidOperationException("Node not in DLL");
            // Remove the specified node from the DoublyLinkedList<T>.
            Node<T> before = current.Previous; //create before and after nodes
            Node<T> after = current.Next; 
            before.Next = after; //re-point nodes from either side to each other
            after.Previous = before;
            Count--; // decrement Count
        }

        public void RemoveFirst()
        {
            if (isEmpty()) throw new InvalidOperationException();           
            Remove(After(Head)); //remove first node after head
        }

        public void RemoveLast()
        {
            if (isEmpty()) throw new InvalidOperationException();           
            Remove(Before(Tail)); //remove first node before Tail
        }

        //----other methods
        public bool isEmpty(){
            if (Head.Next == Tail) return true;    
            else return false;        
        }
    }
}
