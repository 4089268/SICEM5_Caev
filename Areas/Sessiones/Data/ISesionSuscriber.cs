using System;
using System.Collections.Generic;

namespace Sicem_Blazor.Sessiones.Data {
    public interface ISesionSuscriber {
        public string IDSesionSuscriber {get;set;}
        public void SessionAdded();
    }

    public interface ISessionPublisher {
        public void AddSuscriber(ISesionSuscriber s);
        public void RemoveSuscriber(string IDSesionSuscriber);
        public void NotifySuscribers();
    }

}