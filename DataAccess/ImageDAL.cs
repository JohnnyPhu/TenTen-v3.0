using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ImageDAL
    {
        TenTenDataContext db;

        public ImageDAL()
        {
            db = new TenTenDataContext();
        }

        public Image getImageOfWord(string Word)
        {
            return db.Images.Where(i => i.WordID == Word).FirstOrDefault();
        }

        public List<Image> getImageOfWordList(List<Word> wordList)
        {
            List<Image> imgList = new List<Image>();
            foreach(Word w in wordList)
            {
                Image i = db.Images.Where(x => x.WordID == w.Word1).FirstOrDefault();
                imgList.Add(i);
            }
            return imgList;
        }
        //lay hinh ngau nhien theo loai
        public Image getRamdomImageByCategory(int categoryId)
        {
            Image img = new Image();
            Random rand = new Random();
            List<Word> lstWord = db.Words.Where(x => x.CategoryID == categoryId).ToList<Word>();
            int toSkip = rand.Next(0, lstWord.Count());
            string word = lstWord.Where(x=>x.CategoryID==categoryId).Skip(toSkip).Take(1).First().Word1;
            img = db.Images.Where(x => x.WordID == word).FirstOrDefault();
            return img;
        }
    }
}
