/*
Données de base des lignées/Joueurs.
*/

namespace PVP.DataPlayer
{
    public class Carac
    {
        public int physique {get;set;}
        public int magie {get;set;}
    }

    public class Equip {
        public string slot1 {get;set;}
        public string slot2{get;set;}
    }

    public class Capa
    {
        public string slot1 { get; set; }
        public string slot2 { get; set; }
    }

    public class Desc {
        public string name {get;set;}
        public int gen {get;set;}
        public Carac carac {get;set;}
        public Equip equip {get;set;}
        public Capa capa {get;set;}
    }

    [System.Serializable]
    public class Linea{
        public int ActualPlayerID {get;set;}
        public int LineaID {get;set;}
        public string name{get;set;}
        public Desc desc {get;set;}
    }
}