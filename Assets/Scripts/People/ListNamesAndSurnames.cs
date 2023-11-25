using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListNamesAndSurnames
{
    static string[] maleNames = { "Adriano", "Arcadio", "Aurelio","Balbino","Casio",
        "Claudio","Domicio","Elio","Constancio","Julio","Lucio","Máximo","Tito"};
    static string[] femaleNames= { "Antonia", "Celia", "Agripina","Elvia","Dionisia",
        "Fabia","Fausta","Decia","Claudia","Desideria","Calvina","Adriana"};
    static List<string> familySurnames = new List<string>() {"Villa","Esposito","Gallo","Fontana","Russo","Rossi","Spurius","Marcus",
        "Sextus","Tiberius","Cornicen","Crispo","Rufo","Verres","Publicola","Craso","Camilo","Atella","Agripa","Acuelo"};
    static string lastFamilySurname;

    public static string[] ReturnNamesAndSurnames(bool isMale,bool changeFamily)
    {
        if (changeFamily)   //if the family is changed, then makes a new family surname
        {
            int randomSurname = Random.Range(0, familySurnames.Count);
            lastFamilySurname = familySurnames[randomSurname];
            familySurnames.RemoveAt(randomSurname);
            if (isMale)
            {
                int randomNameMale = Random.Range(0, maleNames.Length);
                string nameMale = maleNames[randomNameMale];
                Debug.Log(nameMale);

                return new string[] {nameMale,lastFamilySurname};
            }
            else
            {
                int randomNameFemale = Random.Range(0, femaleNames.Length);
                string nameFemale = femaleNames[randomNameFemale];
                return new string[] { nameFemale, lastFamilySurname };
            }
        }
        else
        {
            if (isMale)
            {
                int randomNameMale = Random.Range(0, maleNames.Length);
                string nameMale = maleNames[randomNameMale];
                Debug.Log(nameMale);

                return new string[] { nameMale, lastFamilySurname };
            }
            else
            {
                int randomNameFemale = Random.Range(0, femaleNames.Length);
                string nameFemale = femaleNames[randomNameFemale];
                return new string[] { nameFemale, lastFamilySurname };
            }
        }
    }

}
