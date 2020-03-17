using System.Collections.Generic;

namespace DynamicDataLibrary
{
    public class MyDataInfo
    {
        public static Dictionary<string, List<string>> CreateData(int categorys, int entitys)
        {
            Dictionary<string, List<string>> CategoryNames = new Dictionary<string, List<string>>();
            CategoryNames.Clear();
          
            int id = 0;
            for (int iCategory=0; iCategory < categorys; ++iCategory)
            {
                string categoryName = "category" + id.ToString();
                id++;
                var names = new List<string>();
                for (int iEntity = 0; iEntity < entitys; ++iEntity)
                {
                    string entityName = "entity" + id.ToString();
                    names.Add(entityName);
                }
                CategoryNames[categoryName] = names;
            }

            return CategoryNames;
        }
    }
}
