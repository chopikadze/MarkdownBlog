using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OC
{
    public class TestItem : INotifyPropertyChanged
    {
        public TestItem(int value)
        {
            Value = value;
        }

        #region Value

        private int _Value;

        public int Value
        {
            get { return _Value; }
            set
            {
                //ignore if values are equal
                if (_Value == value) return;

                _Value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged<TSource>(Expression<Func<TSource>> propertyExpression)
        {
            var propertyName = ((MemberExpression)propertyExpression.Body).Member.Name;

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
