using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
#if !SERVER
using UnityEngine.UI;
# endif
namespace Utils
{
	class DictionaryRepresentationConvention : ConventionBase, IMemberMapConvention
	{
		private readonly DictionaryRepresentation _dictionaryRepresentation;
		public DictionaryRepresentationConvention(DictionaryRepresentation dictionaryRepresentation)
		{
			_dictionaryRepresentation = dictionaryRepresentation;
		}
		public void Apply(BsonMemberMap memberMap)
		{
			memberMap.SetSerializer(ConfigureSerializer(memberMap.GetSerializer()));
		}
		private IBsonSerializer ConfigureSerializer(IBsonSerializer serializer)
		{
			var dictionaryRepresentationConfigurable = serializer as IDictionaryRepresentationConfigurable;
			if (dictionaryRepresentationConfigurable != null)
			{
				serializer = dictionaryRepresentationConfigurable.WithDictionaryRepresentation(_dictionaryRepresentation);
			}

			var childSerializerConfigurable = serializer as IChildSerializerConfigurable;
			return childSerializerConfigurable == null
				? serializer
				: childSerializerConfigurable.WithChildSerializer(ConfigureSerializer(childSerializerConfigurable.ChildSerializer));
		}
	}
	public static class CollectionExtension
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				var k = ET.RandomHelper.RandomNumber(0, n--);
				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static int FindIndex<T>(this List<T> list, T item, IEqualityComparer<T> comparer)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (comparer.Equals(list[i], item)) return i;
			}

			return -1;
		}

		public static T RemoveLast<T>(this List<T> list)
		{
			var last = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return last;
		}
#if !SERVER

		public static void Print(this Text text, string content, bool append = false)
		{
			if (append)
				text.text += "\n" + content;
			else
				text.text = content;
		}
#endif
		public static void Subtract<T>(this List<T> list, T[] subList, T except)
		{
			int index = Array.FindIndex(subList, item => item.Equals(except));
			for (int i = 0; i < subList.Length; i++)
			{
				if (i == index) continue;
				list.Remove(subList[i]);
			}
		}

		public static void Subtract<T>(this List<T> list, IEnumerable<T> subList)
		{
			foreach (var item in subList)
			{
				list.Remove(item);
			}
		}
	}
}