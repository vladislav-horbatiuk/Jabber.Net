namespace agsXMPP.protocol.iq.blocklist
{
    using Xml.Dom;

    public class Blocklist: BlockBase
    {
        public Blocklist():base()
        {
            TagName = "blocklist";
        }
    }

    public class Block : BlockBase
    {
        public Block()
            : base()
        {
            TagName = "block";
        }
    }

    public class Unblock : BlockBase
    {
        public Unblock()
            : base()
        {
            TagName = "unblock";
        }
    }

    public class BlockBase : Element
    {
        public BlockBase()
		{
            this.Namespace	= Uri.IQ_BLOCKLIST;
		}


        public BlockItem[] GetItems()
        {
            ElementList nl = SelectElements(typeof(BlockItem));
            int i = 0;
            BlockItem[] result = new BlockItem[nl.Count];
            foreach (BlockItem ri in nl)
            {
                result[i] = (BlockItem)ri;
                i++;
            }
            return result;
        }

        public void AddBlockItem(BlockItem r)
        {
            this.ChildNodes.Add(r);
        }
    }
}