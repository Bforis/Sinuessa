/*
Données de base des lignées/Joueurs.
*/

namespace DataPlayer
{

    public class Carac
    {
        public string physique { get; set; }
        public string magie { get; set; }
    }

    public class Equip
    {
        public string slot1 { get; set; }
        public string slot2 { get; set; }
    }

    public class Obj
    {
        public string slot1 { get; set; }
        public string slot2 { get; set; }
    }

    public class Capa
    {
        public string slot1 { get; set; }
        public string slot2 { get; set; }
    }

    public class Desc
    {
        public string name { get; set; }
        public string gen { get; set; }
        public Carac carac { get; set; }
        public Equip equip { get; set; }
        public Obj obj { get; set; }
        public Capa capa { get; set; }
    }
    [System.Serializable]
    public class Lignee
    {
        public int ActualPlayerID { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Desc desc { get; set; }

    }

}