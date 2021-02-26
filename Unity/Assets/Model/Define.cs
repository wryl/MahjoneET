namespace ET
{
	public static class Define
	{
#if UNITY_EDITOR && !ASYNC
		public static bool IsAsync = true;
#else
        public static bool IsAsync = true;
#endif
	}
}