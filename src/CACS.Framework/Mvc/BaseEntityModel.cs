using CACS.Framework.Mvc.Filters;
using System.ComponentModel;

namespace CACS.Framework.Mvc
{
    public abstract class BaseEntityModel : BaseModel
    {
        [DisplayName("ID"), LogField(-1)]
        public virtual object Id { get; set; }
    }

    public abstract class BaseEntityModel<T> : BaseEntityModel
    {
        [DisplayName("ID"), LogField(-1)]
        new public virtual T Id
        {
            get
            {
                if (base.Id == null)
                {
                    base.Id = default(T);
                }
                return (T)base.Id;
            }
            set { base.Id = value; }
        }
    }
}
