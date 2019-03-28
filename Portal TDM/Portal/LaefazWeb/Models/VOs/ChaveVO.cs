namespace TDMWeb.Models.VOs
{
    public class ChaveVO
    {
        public int? key { get; set; }

        public string value { get; set; }

        public ChaveVO(string value, int key)
        {
            this.value = value;
            this.key = key;
        }
        public ChaveVO()
        {

        }
    }
}