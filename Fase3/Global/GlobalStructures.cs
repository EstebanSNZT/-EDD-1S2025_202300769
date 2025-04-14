using Structures;

namespace Global
{
    public class GlobalStructures
    {
        public static Blockchain UsersBlockchain = new Blockchain();
        public static DoublyLinkedList VehiclesList = new DoublyLinkedList();
        public static AVLTree SparePartsTree = new AVLTree();
        public static BinaryTree ServicesTree = new BinaryTree();
        public static UndirectedGraph Graph = new UndirectedGraph();
        public static MerkleTree InvoicesTree = new MerkleTree();
    }
}