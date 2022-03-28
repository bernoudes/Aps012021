using App.Services.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace APS_01_2021.Models
{
    [NotMapped]
    public class FrontAction
    {
        //WhoCtrl : which controller send the message
        public string WhoCtrl { get; set; }
        //ActSolicited : what action the front need to do
        public string ActSolicited { get; set; }
        //ExtraData : recommend send in format like json is more easy for the js understand
        public string ExtraData { get; set; }

        public FrontAction()
        {
        }

        public FrontAction(string whoCtrl, string actSolicited, object extraData)
        {
            FrontActionValues(whoCtrl, actSolicited, extraData);
        }

        public void FrontActionValues(string whoCtrl, string actSolicited, object extraData)
        {
            WhoCtrl = whoCtrl;
            ActSolicited = actSolicited;
            try
            {
                ExtraData = JsonSerializer.Serialize(extraData);
            }
            catch
            {
                throw new IntegrityException("ExtraDataFormatErro( correct way => new{data: data})");
            }
        }
    }
}
