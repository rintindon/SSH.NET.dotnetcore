﻿using System;

namespace Renci.SshNetCore.Security.Chaos.NaCl.Internal.Ed25519Ref10
{
	internal static partial class GroupOperations
	{
		internal static void ge_p2_0(out  GroupElementP2 h)
		{
			FieldOperations.fe_0(out h.X);
			FieldOperations.fe_1(out h.Y);
			FieldOperations.fe_1(out h.Z);
		}
	}
}