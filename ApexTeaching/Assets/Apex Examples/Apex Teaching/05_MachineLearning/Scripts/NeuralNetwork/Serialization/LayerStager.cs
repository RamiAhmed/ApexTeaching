namespace Apex.AI.NeuralNetwork
{
    using System;
    using System.Collections;
    using System.Linq;
    using Apex.Serialization;

    public sealed class LayerStager : IStager
    {
        public Type[] handledTypes
        {
            get { return new[] { typeof(Layer) }; }
        }

        public StageItem StageValue(string name, object value)
        {
            var list = value as IList;
            var count = list.Count;
            var listElement = new StageList(name);

            for (int i = 0; i < count; i++)
            {
                var item = SerializationMaster.Stage("Item", list[i]);
                listElement.Add(item);
            }

            return listElement;
        }

        public object UnstageValue(StageItem item, Type targetType)
        {
            var el = (StageList)item;
            var items = el.Items().ToArray();

            var list = Activator.CreateInstance(targetType, items.Length) as IList;
            for (int i = 0; i < items.Length; i++)
            {
                var itemValue = SerializationMaster.Unstage(items[i], typeof(Neuron));
                list.Add(itemValue);
            }

            return list;
        }
    }
}