using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace ClientCSharpConcole
{
	using YieldCurveSrv;

	class Program
	{
		private static void Main(string[] args)
		{
			const string endpointAddress = "http://localhost:58596/YieldCurveSrv.svc";

			// this is the client code.. simply put this in another console app
			BasicHttpBinding binding = new BasicHttpBinding();
			using (var factory = new ChannelFactory<IYieldCurveSrv>(binding, endpointAddress))
			{
				binding.MaxReceivedMessageSize = int.MaxValue;
				binding.MaxBufferSize = int.MaxValue;  

				IYieldCurveSrv channel = factory.CreateChannel();

				ResponseInitData res = channel.InitData();

				Console.WriteLine(String.Format("It is {0}", "Ok"));

				ResponseGetAllEntryPoints allEPs = channel.GetAllEntryPoints();

				Console.WriteLine(String.Format("It is {0}", "Ok"));
			}
		}
	}
}
