using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Domain.Enums
{
	public enum OrderSkuStatus
	{
		Allocated = 0,
		CancelledByUser = 1,
		CancelledBySkuCorrection = 2
	}
}
