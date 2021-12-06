using App.Common;

namespace App.Models
{
    public class Suffix : Entity
    {
        public static readonly Suffix Jr = new Suffix(1L, "jr");
        public static readonly Suffix Sr = new Suffix(2L, "sr");

        public static readonly Suffix[] AllSufixes = { Jr, Sr };

        public string Name { get; set; }

        protected Suffix()
        {

        }

        private Suffix(long id, string name)
            : base(id)
        {
            Name = name;
        }

        public static Suffix? FromId(long id)
        {
            return AllSufixes.SingleOrDefault(s => s.Id == id);
        }
    }
}
