using System.Globalization;

namespace Sicem_Blazor.SeguimientoCobros.Models
{
    public class OfficePushpinMap {

        public int Id {get;set;}
        public string Title {
            get {
                if(Income == 0){
                    return "0";
                }
                return Income.ToString("c2", new CultureInfo("es-MX") );
            }
        }
        public string Office {get;set;}
        public string Lat {get;set;}
        public string Lon {get;set;}
        public int Bills {get;set;}
        public decimal Income {get;set;}
        

        public OfficePushpinMap(int id, string office){
            this.Id = id;
            this.Office = office;
            this.Lat = "25.184467";
            this.Lon = "-102.767124";
            this.Bills = 0;
            this.Income = 0m;
        }

    }
}