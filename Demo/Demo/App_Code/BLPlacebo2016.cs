//Demo BL-Logiikka oppilas-olioille
using System;
using System.Collections.Generic;

namespace JAMK.ICT.BL
{
  public class Oppilas
  {
    public string EtuNimi { get; set; }
    public string SukuNimi { get; set; }
    public string AsioID { get; set; }
    public Oppilas(string etunimi, string sukunimi)
    {
      this.EtuNimi = etunimi;
      this.SukuNimi = sukunimi;
      //keksitään asioid hatusta
      this.AsioID = "X"
                  + etunimi.Substring(0, 1)
                  + sukunimi.Substring(0, 1)
                  + (int)sukunimi[0];
    }
    public override string ToString()
    {
      return this.EtuNimi + " " + this.SukuNimi + "," + this.AsioID;
    }
  }
  //for testing purpose only
  public class Oppilaat
  {
    List<Oppilas> oppilaat;
    public Oppilaat()
    {
      oppilaat = new List<Oppilas>();
    }
    public List<Oppilas> Hae3TestiOppilasta()
    {
      //lisätään kolme keksitty oppilasta
      oppilaat.Add(new Oppilas("Jack", "Daniels"));
      oppilaat.Add(new Oppilas("Jim", "Beam"));
      oppilaat.Add(new Oppilas("Famous", "Grouse"));
      oppilaat.Add(new Oppilas("Kalle", "Kehveli"));

      return oppilaat;
    }
  }
}
