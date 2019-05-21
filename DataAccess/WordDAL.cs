using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class WordDAL
    {
        TenTenDataContext db;
    
        public WordDAL()
        {
            db = new TenTenDataContext();
        }
        
        public List<Word> getWordByCategory(int CategoryID)
        {
            List<Word> list = new List<Word>();
            list = db.Words.Where(w => w.CategoryID == CategoryID).ToList<Word>();
            return list;
        }

        public Word getWord(string word)
        {
            Word w = db.Words.Single(x => x.Word1 == word);
            return w;
        }

        public List<Word> getRandomWordForPairGame(int CategoryID)
        {
            List<Word> list = new List<Word>();
            list = db.Words.Where(w => w.CategoryID == CategoryID).ToList<Word>();
            List<Word> ChosenList = new List<Word>();
            Random rand = new Random();
            for(int i=0;i<8;i++)
            {
                int toSkip = rand.Next(0, list.Count);
                Word w = list.Where(x=>x.CategoryID == CategoryID).Skip(toSkip).Take(1).First();
                list.Remove(w);
                ChosenList.Add(w);
            }
           
            return ChosenList;
        }

        public List<Word> getRandomWordForLearningWord(int CategoryID)
        {
            List<Word> list = new List<Word>();
            list = db.Words.Where(w => w.CategoryID == CategoryID).ToList<Word>();
            List<Word> ChosenList = new List<Word>();
            Random rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int toSkip = rand.Next(0, list.Count);
                Word w = list.Where(x => x.CategoryID == CategoryID).Skip(toSkip).Take(1).First();
                list.Remove(w);
                ChosenList.Add(w);
            }

            return ChosenList;
        }
        public Word geRandomtWordByCategory(int categoryId)
        {
            Word word = new Word();
            Random rand = new Random();
            List<Word> lst = new List<Word>();
            lst = db.Words.Where(s => s.CategoryID == categoryId).ToList<Word>();
            int toSkip = rand.Next(0, lst.Count());
            word = lst.Where(x => x.CategoryID == categoryId).Skip(toSkip).Take(1).First();
            return word;

        }
    }
}
