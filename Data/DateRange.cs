using System;

namespace Sicem_Blazor.Data{
    public class DateRange{
        private DateTime _desde;
        private DateTime _hasta;
        private int _sb = 0;
        private int _sect = 0;

        public DateTime Desde { get => _desde; }
        public DateTime Hasta { get => _hasta; }
        public int Subsistema { get => _sb; }
        public int Sector { get => _sect; }

        public string Desde_ISO { get => _desde.ToString("yyyyMMdd"); }
        public string Hasta_ISO { get => _hasta.ToString("yyyyMMdd"); }
        
        public DateRange(DateTime desde, DateTime hasta){
            this._desde = desde;
            this._hasta = hasta;
            this._sb = 0;
            this._sect = 0;
        }
        public DateRange(DateTime desde, DateTime hasta, int sb){
            this._desde = desde;
            this._hasta = hasta;
            this._sb = sb;
            this._sect = 0;
        }
        public DateRange(DateTime desde, DateTime hasta, int sb, int sect){
            this._desde = desde;
            this._hasta = hasta;
            this._sb = sb;
            this._sect = sect;
        }

    }
}