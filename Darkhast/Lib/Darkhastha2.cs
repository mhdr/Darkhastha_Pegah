using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Darkhast.Lib
{
    class Darkhastha2
    {
        private string _vaziatName;
        private string _barghkarFullName;
        private Guid _barghkarGUID;
        private Barghkarha _barghkarha;
        private Guid _darkhastGUID;
        private string _darkhastName;
        private Bakhshha _bakhshha;
        private string _bakhshName;
        private Guid? _dastgahGUID;
        private Dastgahha _dastgahha;
        private string _dastgahName;
        private int _isTrash;
        private string _shomareFani;
        private DateTime? _tarikh;
        private string _tarikh2;
        private int _tedadDarkhast;
        private string _tozihat;
        private string _vahedShomaresh;
        private int _vaziat;
        private DateTime? _tarikhDaryaftKala;
        private string _tarikhDaryaftKala2;
        private string _shomareDarkhastTadbir;

        public Darkhastha2(Darkhastha darkhast)
        {
            try
            {
                this.BarghkarGuid = darkhast.BarghkarGUID;
                this.Barghkarha = darkhast.Barghkarha;
                this.DarkhastGuid = darkhast.DarkhastGUID;
                this.DarkhastName = darkhast.DarkhastName;
                this.DastgahGuid = darkhast.DastgahGUID;
                if (darkhast.Dastgahha != null)
                {
                    this.DastgahName = darkhast.Dastgahha.DastgahName;
                    this.BakhshName = darkhast.Dastgahha.Bakhshha.BakhshName;
                }

                this.IsTrash = darkhast.IsTrash;
                this.ShomareFani = darkhast.ShomareFani;

                PersianDate persianDate = new PersianDate((DateTime)darkhast.Tarikh);
                this.Tarikh = persianDate.ConvertMiladiToShamsi();
                DateTime date = persianDate.ConvertMiladiToShamsi();
                this.Tarikh2 = string.Format("{0}/{1:d2}/{2:d2}", date.Year, date.Month, date.Day);

                if (darkhast.TarikhDaryaftKala != null)
                {
                    PersianDate persianDateForDaryaft = new PersianDate((DateTime)darkhast.TarikhDaryaftKala);
                    this.TarikhDaryaftKala = persianDateForDaryaft.ConvertMiladiToShamsi();
                    DateTime dateDaryaft = persianDateForDaryaft.ConvertMiladiToShamsi();
                    this.TarikhDaryaftKala2 = string.Format("{0}/{1:d2}/{2:d2}", dateDaryaft.Year, dateDaryaft.Month, dateDaryaft.Day);
                }
                else
                {
                    this.TarikhDaryaftKala2 = "";
                }


                this.TedadDarkhast = darkhast.TedadDarkhast;
                this.Tozihat = darkhast.Tozihat;
                this.VahedShomaresh = darkhast.VahedShomaresh;
                this.Vaziat = darkhast.Vaziat;
                this.ShomareDarkhastTadbir = darkhast.ShomareDarkhastTadbir;


                SetVaziat(this.Vaziat);
                SetBarghkarFullName(this.Barghkarha.FirstName, this.Barghkarha.LastName);
            }
            catch (Exception)
            {

            }
        }

        private void SetBarghkarFullName(string firstName, string lastName)
        {
            BarghkarFullName = string.Format("{0} {1}", firstName, lastName);
        }

        private void SetVaziat(int vaziat)
        {
            switch (vaziat)
            {
                case (int)VaziatDarkhast.DarHaleBarresi:
                    VaziatName = "در حال بررسي";
                    break;
                case (int)VaziatDarkhast.TaeedNashode:
                    VaziatName = "تائيد نشده";
                    break;
                case (int)VaziatDarkhast.TaeedShode:
                    VaziatName = "تائيد شده";
                    break;
            }
        }

        public string VaziatName
        {
            get { return _vaziatName; }
            set { _vaziatName = value; }
        }

        public string BarghkarFullName
        {
            get { return _barghkarFullName; }
            set { _barghkarFullName = value; }
        }

        public Guid BarghkarGuid
        {
            get { return _barghkarGUID; }
            set { _barghkarGUID = value; }
        }

        public Barghkarha Barghkarha
        {
            get { return _barghkarha; }
            set { _barghkarha = value; }
        }

        public Guid DarkhastGuid
        {
            get { return _darkhastGUID; }
            set { _darkhastGUID = value; }
        }

        public string DarkhastName
        {
            get { return _darkhastName; }
            set { _darkhastName = value; }
        }

        public Guid? DastgahGuid
        {
            get { return _dastgahGUID; }
            set { _dastgahGUID = value; }
        }

        public Dastgahha Dastgahha
        {
            get { return _dastgahha; }
            set { _dastgahha = value; }
        }

        public int IsTrash
        {
            get { return _isTrash; }
            set { _isTrash = value; }
        }

        public string ShomareFani
        {
            get { return _shomareFani; }
            set { _shomareFani = value; }
        }

        public DateTime? Tarikh
        {
            get { return _tarikh; }
            set { _tarikh = value; }
        }

        public int TedadDarkhast
        {
            get { return _tedadDarkhast; }
            set { _tedadDarkhast = value; }
        }

        public string Tozihat
        {
            get { return _tozihat; }
            set { _tozihat = value; }
        }

        public string VahedShomaresh
        {
            get { return _vahedShomaresh; }
            set { _vahedShomaresh = value; }
        }

        public int Vaziat
        {
            get { return _vaziat; }
            set { _vaziat = value; }
        }

        public string Tarikh2
        {
            get { return _tarikh2; }
            set { _tarikh2 = value; }
        }

        public DateTime? TarikhDaryaftKala
        {
            get { return _tarikhDaryaftKala; }
            set { _tarikhDaryaftKala = value; }
        }

        public string TarikhDaryaftKala2
        {
            get { return _tarikhDaryaftKala2; }
            set { _tarikhDaryaftKala2 = value; }
        }

        public Bakhshha Bakhshha
        {
            get { return _bakhshha; }
            set { _bakhshha = value; }
        }

        public string ShomareDarkhastTadbir
        {
            get { return _shomareDarkhastTadbir; }
            set { _shomareDarkhastTadbir = value; }
        }

        public string BakhshName
        {
            get { return _bakhshName; }
            set { _bakhshName = value; }
        }

        public string DastgahName
        {
            get { return _dastgahName; }
            set { _dastgahName = value; }
        }
    }

}
