using CHBackOffice.ApiServices.ChsProxy;
using CHSBackOffice.Support.StaticResources;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CHSBackOffice.Support.Classes
{
    public class KioskExtended : ListViewItemBase<Kiosk>
    {
        #region Public Properties
        public Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string> DataSet
        {
            get 
            {
                if (BaseObject.IsRecycler)
                    return new Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>(
                    new List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>
                    {
                        GetRecycler1(),
                        GetRecycler2(),
                        GetCoinHopper()
                    },
                    BaseObject.State,
                    BaseObject.Status,
                    BaseObject.Id);

                return new Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>(
                    new List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>
                    { 
                        GetCashDispenser(),
                        GetCoinHopper(),
                        GetBillAcceptor()
                    },
                    BaseObject.State,
                    BaseObject.Status,
                    BaseObject.Id);
            }
        }

        public Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string> DataSetWithoutId
        {
            get
            {
                if (BaseObject.IsRecycler)
                    return new Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>(
                    new List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>
                    {
                        GetRecycler(),
                        GetCoinHopper()
                    },
                    BaseObject.State,
                    BaseObject.Status,
                    string.Empty);

                return new Tuple<List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>, KioskState, KioskStatus, string>(
                    new List<Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>>
                    {
                        GetCashDispenser(),
                        GetCoinHopper(),
                        GetBillAcceptor()
                    },
                    BaseObject.State,
                    BaseObject.Status,
                    string.Empty);
            }
        }
        #endregion

        #region Private Methods
        private Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string> GetCashDispenser()
        {
            List<Tuple<float, float, Func<float, float, Color>>> entities = new List<Tuple<float, float, Func<float, float, Color>>>();

            if (BaseObject != null && BaseObject.CashDispenser != null && BaseObject.CashDispenser.Length > 0)
            {
                for (int i = 0; i < BaseObject.CashDispenser.Length; i++)
                {
                    var item = BaseObject.CashDispenser[i];

                    entities.Add(new Tuple<float, float, Func<float, float, Color>>(item.Count, item.Capacity, CashCoinColorFunc));
                }
            }

            return new Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>(entities, "CASH DIS.");
        }

        private Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string> GetCoinHopper()
        {
            List<Tuple<float, float, Func<float, float, Color>>> entities = new List<Tuple<float, float, Func<float, float, Color>>>();
            if (BaseObject != null && BaseObject.CoinHopper != null && BaseObject.CoinHopper.Length > 0)
            {
                for (int i = 0; i < BaseObject.CoinHopper.Length; i++)
                {
                    var item = BaseObject.CoinHopper[i];
                    entities.Add(new Tuple<float, float, Func<float, float, Color>>(item.Count, item.Capacity, CashCoinColorFunc));
                }
            }
            return new Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>(entities, "COINS DIS.");
        }

        private Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string> GetBillAcceptor()
        {
            List<Tuple<float, float, Func<float, float, Color>>> entities = new List<Tuple<float, float, Func<float, float, Color>>>();
            if (BaseObject != null && BaseObject.BillAcceptor != null && BaseObject.BillAcceptor.Length > 0)
            {
                for (int i = 0; i < BaseObject.BillAcceptor.Length; i++)
                {
                    var item = BaseObject.BillAcceptor[i];
                    entities.Add(new Tuple<float, float, Func<float, float, Color>>(item.Count, item.Capacity, BillColorFunc));
                }
            }
            return new Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>(entities, "BILL VAL.");
        }

        private Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string> GetRecycler1()
        {
            List<Tuple<float, float, Func<float, float, Color>>> entities = new List<Tuple<float, float, Func<float, float, Color>>>();

            if (BaseObject?.RecyclerCassettes != null && BaseObject.RecyclerCassettes.Length > 0)
            {
                var rec1Length = Math.Min(BaseObject.RecyclerCassettes.Length, 4);
                for (int i = 0; i < rec1Length; i++)
                {
                    var item = BaseObject.RecyclerCassettes[i];

                    entities.Add(new Tuple<float, float, Func<float, float, Color>>(item.MediaRemainingUnit, item.Capacity, RecyclerColorFunc));
                }
            }

            return new Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>(entities, "REYCLER");
        }

        private Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string> GetRecycler2()
        {
            List<Tuple<float, float, Func<float, float, Color>>> entities = new List<Tuple<float, float, Func<float, float, Color>>>();

            if (BaseObject?.RecyclerCassettes != null && BaseObject.RecyclerCassettes.Length > 4)
            {
                for (int i = 4; i < BaseObject.RecyclerCassettes.Length; i++)
                {
                    var item = BaseObject.RecyclerCassettes[i];

                    entities.Add(new Tuple<float, float, Func<float, float, Color>>(item.MediaRemainingUnit, item.Capacity, RecyclerColorFunc));
                }
            }

            return new Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>(entities, "REYCLER");
        }

        private Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string> GetRecycler()
        {
            List<Tuple<float, float, Func<float, float, Color>>> entities = new List<Tuple<float, float, Func<float, float, Color>>>();

            if (BaseObject?.RecyclerCassettes != null && BaseObject.RecyclerCassettes.Length > 0)
            {
                for (int i = 0; i < BaseObject.RecyclerCassettes.Length; i++)
                {
                    var item = BaseObject.RecyclerCassettes[i];

                    entities.Add(new Tuple<float, float, Func<float, float, Color>>(item.MediaRemainingUnit, item.Capacity, RecyclerColorFunc));
                }
            }

            return new Tuple<List<Tuple<float, float, Func<float, float, Color>>>, string>(entities, "REYCLER");
        }

        private Func<float, float, Color> CashCoinColorFunc = (count,capacity) =>
        {
            try 
            {
                if (count >= 100)
                {
                    return StaticResourceManager.GetColor("ChartNormal");
                }
                else if (count > 0 && count < 100)
                {
                    return StaticResourceManager.GetColor("ChartWarning");
                }
                else if (count == 0)
                {
                    return StaticResourceManager.GetColor("ChartCritical");
                }
                return default(Color);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return default(Color);
            }
            
        };

        private Func<float, float, Color> BillColorFunc = (count, capacity) =>
        {
            try 
            {
                if (count < (capacity - 100))
                {
                    return StaticResourceManager.GetColor("ChartNormal");
                }
                else if (count >= (capacity - 100))
                {
                    return StaticResourceManager.GetColor("ChartWarning");
                }
                else if (count == capacity)
                {
                    return StaticResourceManager.GetColor("ChartCritical");
                }
                else if (count == 0)
                {
                    return StaticResourceManager.GetColor("ChartCapacity");
                }
                return default(Color);
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return default(Color);
            }
        };

        private Func<float, float, Color> RecyclerColorFunc = (count, capacity) =>
        {
            try
            {
                return StaticResourceManager.GetColor("ChartNormal");
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return default(Color);
            }
        };

        private Func<KioskState, KioskStatus, Color> StateStatusColorFunc = (state, status) => 
        {
            try 
            {
                if (state == CHBackOffice.ApiServices.ChsProxy.KioskState.InService ||
                   state == CHBackOffice.ApiServices.ChsProxy.KioskState.ONLINE)
                {
                    if (status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Normal)
                    {
                        return StaticResourceManager.GetColor("StatusNormal");
                    }
                    else if (status == CHBackOffice.ApiServices.ChsProxy.KioskStatus.Warning)
                    {
                        return StaticResourceManager.GetColor("StatusWarning");
                    }
                    else
                    {
                        return StaticResourceManager.GetColor("StatusCritical");
                    }
                }
                else if (state == CHBackOffice.ApiServices.ChsProxy.KioskState.Offline)
                {
                    return StaticResourceManager.GetColor("StatusOffline");
                }
                else if (state == CHBackOffice.ApiServices.ChsProxy.KioskState.OutOfServiceSOP)
                {
                    return StaticResourceManager.GetColor("StatusSOP");
                }
                else
                {
                    return StaticResourceManager.GetColor("StatusOOS");
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
                return default(Color);
            }
            
        };
        #endregion
    }
}
