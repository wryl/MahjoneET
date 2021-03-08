using System;
using GamePlay.Client.View.SubManagers;
using GamePlay.Server.Model;
using UnityEngine;

namespace GamePlay.Client.View
{
	public class PlayerEffectManager : MonoBehaviour
	{
		public EffectManager[] EffectManagers;

		public float ShowEffect(int placeIndex, Type type)
		{
            switch (type)
            {
                case Type.Richi:
					Managers.AudioManager.Instance.PlayAct("act_rich");
                    break;
                case Type.Chow:
					Managers.AudioManager.Instance.PlayAct("act_chi");
					break;
                case Type.Pong:
					Managers.AudioManager.Instance.PlayAct("act_pon");
					break;
                case Type.Kong:
					Managers.AudioManager.Instance.PlayAct("act_kan");
					break;
                case Type.Bei:
					Managers.AudioManager.Instance.PlayAct("act_babei");
					break;
                case Type.Tsumo:
					Managers.AudioManager.Instance.PlayAct("act_tumo");
					break;
                case Type.Rong:
					Managers.AudioManager.Instance.PlayAct("act_ron");
					break;
                default:
                    break;
            }
            return EffectManagers[placeIndex].StartAnimation(type);
		}

		public static Type GetAnimationType(OutTurnOperationType operation)
		{
			switch (operation)
			{
				case OutTurnOperationType.Chow:
					return Type.Chow;
				case OutTurnOperationType.Pong:
					return Type.Pong;
				case OutTurnOperationType.Kong:
					return Type.Kong;
				case OutTurnOperationType.Rong:
					return Type.Rong;
				default:
					throw new NotSupportedException($"This kind of operation {operation} does not have an animation");
			}
		}

		public enum Type
		{
			Richi,
			Chow,
			Pong,
			Kong,
			Bei,
			Tsumo,
			Rong
		}
	}
}