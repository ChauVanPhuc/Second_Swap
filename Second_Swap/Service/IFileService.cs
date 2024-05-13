namespace Second_Swap.Service
{
    public interface IFileService
    {
        public string SaveImage(IFormFile imgFile);
        public bool Delete(string imgFile);
    }
}
