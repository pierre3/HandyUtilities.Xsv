using System;
using System.Globalization;
using System.Reflection;

namespace HandyUtil.Text.Xsv
{
    public class XsvDataRow<T> : XsvDataRow where T : new()
    {
        public T Fields { get; set; }
        public bool IsAutoBind { get; set; }

        public event EventHandler<XsvFieldUpdatedEventArgs<T>> Attached;
        public event EventHandler<XsvFieldUpdatedEventArgs<T>> Updated;

        public XsvDataRow()
        {
            IsAutoBind = true;
        }

        public XsvDataRow(bool isAutoBind = false)
        {
            this.IsAutoBind = isAutoBind;
        }

        protected override void AttachFields()
        {
            if (IsAutoBind)
            {
                BindToFields();
            }

            var handler = Attached;
            if (handler != null)
            {
                var args = new XsvFieldUpdatedEventArgs<T>(this, Fields);
                handler(this, args);
                return;
            }
        }

        protected override void UpdateFields()
        {
            if (IsAutoBind)
            {
                BindToSource();
            }

            var handler = Updated;
            if (handler != null)
            {
                handler(this, new XsvFieldUpdatedEventArgs<T>(this, Fields));
                return;
            }
        }

        private void BindToSource()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (_items.ContainsKey(prop.Name))
                {
                    _items[prop.Name] = new XsvField(prop.GetValue(Fields, null).ToString());
                }
            }
        }

        private void BindToFields()
        {
            Fields = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!_items.ContainsKey(prop.Name))
                {
                    continue;
                }

                var type = prop.PropertyType;
                if (type.IsEnum)
                {
                    prop.SetValue(Fields, _items[prop.Name].AsEnum(type, Activator.CreateInstance(type)),null);
                    continue;
                }

                var code = Type.GetTypeCode(type);
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (type.GetGenericParameterConstraints()[0].IsEnum)
                    {
                        prop.SetValue(Fields, _items[prop.Name].AsNullableEnum(type),null);
                        continue;
                    }
                    SetNullableProperty(code, prop);
                    continue;
                }
                SetProperty(code, prop);
            }
        }

        private void SetNullableProperty(TypeCode code, PropertyInfo prop)
        {
            switch (code)
            {
                case TypeCode.Boolean:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableBoolean(),null);
                    break;
                case TypeCode.String:
                    break;
                case TypeCode.Char:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableChar(), null);
                    break;
                case TypeCode.DateTime:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableDateTime(), null);
                    break;
                case TypeCode.Int16:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableInt16(NumberStyles.Currency), null);
                    break;
                case TypeCode.Int32:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableInt32(NumberStyles.Currency), null);
                    break;
                case TypeCode.Int64:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableInt64(NumberStyles.Currency), null);
                    break;
                case TypeCode.UInt16:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableUInt16(NumberStyles.Currency), null);
                    break;
                case TypeCode.UInt32:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableUInt32(NumberStyles.Currency), null);
                    break;
                case TypeCode.UInt64:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableUInt64(NumberStyles.Currency), null);
                    break;
                case TypeCode.Single:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableFloat(NumberStyles.Any), null);
                    break;
                case TypeCode.Double:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableDouble(NumberStyles.Any), null);
                    break;
                case TypeCode.Decimal:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableDecimal(NumberStyles.Any), null);
                    break;
                case TypeCode.SByte:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableSByte(NumberStyles.Currency), null);
                    break;
                case TypeCode.Byte:
                    prop.SetValue(Fields, _items[prop.Name].AsNullableByte(NumberStyles.Currency), null);
                    break;
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    break;
            }
        }

        private void SetProperty(TypeCode code, PropertyInfo prop)
        {
            switch (code)
            {
                case TypeCode.Boolean:
                    prop.SetValue(Fields, _items[prop.Name].AsBoolean(default(bool)), null);
                    break;
                case TypeCode.String:
                    prop.SetValue(Fields, _items[prop.Name].AsString(default(string)), null);
                    break;
                case TypeCode.Char:
                    prop.SetValue(Fields, _items[prop.Name].AsChar(default(char)), null);
                    break;
                case TypeCode.DateTime:
                    prop.SetValue(Fields, _items[prop.Name].AsDateTime(default(DateTime)), null);
                    break;
                case TypeCode.Int16:
                    prop.SetValue(Fields, _items[prop.Name].AsInt16(NumberStyles.Currency, default(short)), null);
                    break;
                case TypeCode.Int32:
                    prop.SetValue(Fields, _items[prop.Name].AsInt32(NumberStyles.Currency, default(int)), null);
                    break;
                case TypeCode.Int64:
                    prop.SetValue(Fields, _items[prop.Name].AsInt64(NumberStyles.Currency, default(long)), null);
                    break;
                case TypeCode.UInt16:
                    prop.SetValue(Fields, _items[prop.Name].AsUInt16(NumberStyles.Currency, default(ushort)), null);
                    break;
                case TypeCode.UInt32:
                    prop.SetValue(Fields, _items[prop.Name].AsUInt32(NumberStyles.Currency, default(uint)), null);
                    break;
                case TypeCode.UInt64:
                    prop.SetValue(Fields, _items[prop.Name].AsUInt64(NumberStyles.Currency, default(ulong)), null);
                    break;
                case TypeCode.Single:
                    prop.SetValue(Fields, _items[prop.Name].AsFloat(NumberStyles.Any, default(float)), null);
                    break;
                case TypeCode.Double:
                    prop.SetValue(Fields, _items[prop.Name].AsDouble(NumberStyles.Any, default(double)), null);
                    break;
                case TypeCode.Decimal:
                    prop.SetValue(Fields, _items[prop.Name].AsDecimal(NumberStyles.Any, default(decimal)), null);
                    break;
                case TypeCode.SByte:
                    prop.SetValue(Fields, _items[prop.Name].AsSByte(NumberStyles.Currency, default(sbyte)), null);
                    break;
                case TypeCode.Byte:
                    prop.SetValue(Fields, _items[prop.Name].AsByte(NumberStyles.Currency, default(byte)), null);
                    break;
                case TypeCode.Object:
                case TypeCode.DBNull:
                case TypeCode.Empty:
                default:
                    break;
            }
        }
    }


}