using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Extensions;
using Xunit;

namespace Lykke.Service.GoogleAnalyticsWrapper.Tests
{
    public class DeviceInfoTests
    {
        [Fact]
        public void Is_Android_ClientInfo_Parsed()
        {
            const string clientInfo = "<android>; Model:<E5303>; Os:<android>; Screen:<1080x1776>;";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseClientInfo(clientInfo);

            Assert.Equal("android", deviceInfo.DeviceType);
            Assert.Equal("E5303", deviceInfo.DeviceModel);
            Assert.Equal("1080x1776", deviceInfo.ScreenResolution);
        }

        [Fact]
        public void Is_Ipad_ClientInfo_Parsed()
        {
            const string clientInfo = "iPad; Model:Air 2; Os:9.3.2; Screen:768x1024";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseClientInfo(clientInfo);

            Assert.Equal("iPad", deviceInfo.DeviceType);
            Assert.Equal("Air 2", deviceInfo.DeviceModel);
            Assert.Equal("9.3.2", deviceInfo.OsVersion);
            Assert.Equal("768x1024", deviceInfo.ScreenResolution);
        }

        [Fact]
        public void Is_Iphone_ClientInfo_Parsed()
        {
            const string clientInfo = "iPhone; Model:6 Plus; Os:9.3.2; Screen:414x736";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseClientInfo(clientInfo);

            Assert.Equal("iPhone", deviceInfo.DeviceType);
            Assert.Equal("6 Plus", deviceInfo.DeviceModel);
            Assert.Equal("9.3.2", deviceInfo.OsVersion);
            Assert.Equal("414x736", deviceInfo.ScreenResolution);
        }

        [Fact]
        public void Is_Wrong_ClientInfo_Parsed()
        {
            const string clientInfo = "string";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseClientInfo(clientInfo);

            Assert.Null(deviceInfo.DeviceType);
            Assert.Null(deviceInfo.DeviceModel);
            Assert.Null(deviceInfo.OsVersion);
            Assert.Null(deviceInfo.ScreenResolution);
        }

        [Fact]
        public void Is_Wrong_ClientInfo_Parsed1()
        {
            const string clientInfo = null;
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseClientInfo(clientInfo);

            Assert.Null(deviceInfo.DeviceType);
            Assert.Null(deviceInfo.DeviceModel);
            Assert.Null(deviceInfo.OsVersion);
            Assert.Null(deviceInfo.ScreenResolution);
        }

        [Fact]
        public void Is_Android_UserAgent_Parsed()
        {
            const string userAgent = "DeviceType=android;DeviceModel=Nexus 6P;AndroidVersion=7.0;AppVersion=553;ClientFeatures=1";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseUserAgent(userAgent);

            Assert.Equal("android", deviceInfo.DeviceType);
            Assert.Equal("Nexus 6P", deviceInfo.DeviceModel);
            Assert.Equal("7.0", deviceInfo.OsVersion);
            Assert.Equal("553", deviceInfo.AppVersion);
        }

        [Fact]
        public void Is_Iphone_UserAgent_Parsed()
        {
            const string userAgent = "DeviceType=iPhone;AppVersion=175.19;ClientFeatures=1";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseUserAgent(userAgent);

            Assert.Equal("iPhone", deviceInfo.DeviceType);
            Assert.Null(deviceInfo.DeviceModel);
            Assert.Null(deviceInfo.OsVersion);
            Assert.Equal("175.19", deviceInfo.AppVersion);
        }

        [Fact]
        public void Is_Ipad_UserAgent_Parsed()
        {
            const string userAgent = "DeviceType=iPad;AppVersion=500;ClientFeatures=1";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseUserAgent(userAgent);

            Assert.Equal("iPad", deviceInfo.DeviceType);
            Assert.Null(deviceInfo.DeviceModel);
            Assert.Null(deviceInfo.OsVersion);
            Assert.Equal("500", deviceInfo.AppVersion);
        }

        [Fact]
        public void Is_Browser_UserAgent_Parsed()
        {
            const string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36";
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseUserAgent(userAgent);

            Assert.Equal(userAgent, deviceInfo.RawUserAgent);
            Assert.Null(deviceInfo.DeviceModel);
            Assert.Null(deviceInfo.OsVersion);
            Assert.Null(deviceInfo.AppVersion);
        }

        [Fact]
        public void Is_Empty_UserAgent_Parsed()
        {
            const string userAgent = null;
            var deviceInfo = new DeviceInfo();
            deviceInfo.ParseUserAgent(userAgent);

            Assert.Null(deviceInfo.RawUserAgent);
            Assert.Null(deviceInfo.DeviceModel);
            Assert.Null(deviceInfo.OsVersion);
            Assert.Null(deviceInfo.AppVersion);
        }
    }
}
