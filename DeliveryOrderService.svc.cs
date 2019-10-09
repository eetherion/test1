using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Core.Manager;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Web;
using System.Device.Location;
using System.Globalization;


namespace TransportService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DeliveryOrderService" in code, svc and config file together.
    public class DeliveryOrderService : IDeliveryOrderService
    {
        //API Reason
        public Stream GetReason()
        {
            string json = Core.Services.RestPublisher.Serialize(ReasonFacade.LoadReason());//wawan
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Core.Model.mdlResultSvc DeleteReason(Core.Model.mdlReason param)
        {
            return ReasonFacade.DeleteReason2(param.ReasonID);
        }

        public Core.Model.mdlResultSvc UploadReason(Core.Model.mdlReason param)
        {
            return ReasonFacade.UploadReason2(param);
        }

        public Stream GetJson(Core.Model.mdlParam json)
        {
            var result = new Core.Model.mdlResultSvc();
            result.Title = "Get Json Download";
			string aa = ""; //test chris
            var resultJson = new Core.Model.mdlJsonList();
            resultJson = JsonFacade.LoadJson(json);
			
            if (resultJson.mdlJson.FirstOrDefault().CallPlanList.Count > 0)
            {
                result.StatusCode = "01";
                result.StatusMessage = "Success";
            }
            else
            {
                result.StatusCode = "00";
                result.StatusMessage = "Failed";
            }
            result.Value = resultJson;
            var strJson = Core.Services.RestPublisher.Serialize(resultJson);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";


            Core.Manager.LogFacade.InsertLog("DownloadJson", json.EmployeeID, result.StatusMessage, json.BranchID, json.DeviceID, sizeKB);


            string serializeJson = Core.Services.RestPublisher.Serialize(result);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(serializeJson));
            return ms;
        }

        public Core.Model.mdlResult CheckConnection()
        {
            var result = new Core.Model.mdlResult();
            result.Result = "1";

            return result;
        }

        public Core.Model.mdlResultList InsertReturOrder(List<Core.Model.mdlReturOrderParam> lParamlist)
        {
            string serlJson = JsonConvert.SerializeObject(lParamlist);
            string lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;
			string xxx="xxx tambah";
            var resultInsertRetur = ReturFacade.InsertReturOrder(lParamlist);
            string JsonResultlist = JsonConvert.SerializeObject(resultInsertRetur);

            Core.Manager.LogFacade.InsertLog("DownloadJson", lEmployeeID, JsonResultlist, lParamlist.FirstOrDefault().BranchID, "", "");

            return resultInsertRetur;
        }

        public Core.Model.mdlResultList InsertReturOrderDetail(List<Core.Model.mdlReturOrderDetailParam> lParamlist)
        {
            string serlJson = JsonConvert.SerializeObject(lParamlist);
            string lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;
			string aa = ""; // test chris

            var resultInsertReturDetail = ReturFacade.InsertReturOrderDetail(lParamlist);
            string JsonResultlist = JsonConvert.SerializeObject(resultInsertReturDetail);

            Core.Manager.LogFacade.InsertLog("DownloadJson", lEmployeeID, JsonResultlist, "", "", "");

            return resultInsertReturDetail;
        }

        public Core.Model.mdlResultList UpdateRetur(List<Core.Model.mdlReturOrderParam> lParamlist)
        {
            string serlJson = JsonConvert.SerializeObject(lParamlist);
            string lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;

            var resultRetur = ReturFacade.UpdateReturOrder(lParamlist);
            string JsonResultlist = JsonConvert.SerializeObject(resultRetur);

            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            Core.Manager.LogFacade.InsertLog("UpdateReturOrder", lEmployeeID, JsonResultlist, lParamlist.FirstOrDefault().BranchID, "", "");

            return resultRetur;
        }

        public Core.Model.mdlResultList UpdateReturDetail(List<Core.Model.mdlReturOrderDetailParam> lParamlist)
        {
            string serlJson = JsonConvert.SerializeObject(lParamlist);
            string lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;

            var resultReturDetail = ReturFacade.UpdateReturOrderDetail(lParamlist);
            string JsonResultlist = JsonConvert.SerializeObject(resultReturDetail);

            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            Core.Manager.LogFacade.InsertLog("UpdateReturOrderDetail", lEmployeeID, JsonResultlist, "", "", "");

            return resultReturDetail;
        }

        public Core.Model.mdlResultList InsertCustomerImage(List<Core.Model.mdlCustomerImageParam> lParamlist)
        {
            string serlJson = JsonConvert.SerializeObject(lParamlist);
            string lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;

            var resultCustomerImage = CustomerImageFacade.InsertCustomerImage(lParamlist);

            string JsonResultlist = JsonConvert.SerializeObject(resultCustomerImage);
            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            var strJson = Core.Services.RestPublisher.Serialize(lParamlist);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";
            Core.Manager.LogFacade.InsertLog("InsertCustomerImage", lEmployeeID, JsonResultlist, lParamlist.FirstOrDefault().BranchID, lParamlist.FirstOrDefault().deviceID, sizeKB);

            return resultCustomerImage;
        }

        public Core.Model.mdlResultList UpdateDeliveryOrder(List<Core.Model.mdlDeliveryOrderParam> lDOParamlist)
        {
            //string serlJson = JsonConvert.SerializeObject(lDOParamlist);
            string lEmployeeID = lDOParamlist.FirstOrDefault().EmployeeID;

            var resultlist = DeliveryOrderFacade.UpdateDeliveryOrder(lDOParamlist);
            string JsonResultlist = JsonConvert.SerializeObject(resultlist);

            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            var strJson = Core.Services.RestPublisher.Serialize(lDOParamlist);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";
            Core.Manager.LogFacade.InsertLog("UpdateDeliveryOrder", lEmployeeID, JsonResultlist, lDOParamlist.FirstOrDefault().BranchID, lDOParamlist.FirstOrDefault().deviceID, sizeKB);

            return resultlist;
        }

        public Core.Model.mdlResultList UpdateDeliveryOrderDetail(List<Core.Model.mdlDeliveryOrderDetailParam> lDODetailParamlist) //005
        {
            string serlJson = JsonConvert.SerializeObject(lDODetailParamlist);
            string lEmployeeID = lDODetailParamlist.FirstOrDefault().EmployeeID;



            var resultDO = DeliveryOrderFacade.UpdateDeliveryOrderDetail(lDODetailParamlist);

            string JsonResultlist = JsonConvert.SerializeObject(resultDO);
            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            var strJson = Core.Services.RestPublisher.Serialize(lDODetailParamlist);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";
            Core.Manager.LogFacade.InsertLog("UpdateDeliveryOrderDetail", lEmployeeID, JsonResultlist, lDODetailParamlist.FirstOrDefault().BranchID, lDODetailParamlist.FirstOrDefault().deviceID, sizeKB);

            return resultDO;
        }

        public Core.Model.mdlResultList InsertVisit(List<Core.Model.mdlVisitParam> lVisitParamlist)
        {
            //string serlJson = JsonConvert.SerializeObject(lVisitParamlist);
            string lEmployeeID = lVisitParamlist.FirstOrDefault().EmployeeID;



            var resultVisit = VisitFacade.InsertVisit(lVisitParamlist);

            string JsonResultlist = JsonConvert.SerializeObject(resultVisit);
            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            var strJson = Core.Services.RestPublisher.Serialize(lVisitParamlist);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";
            Core.Manager.LogFacade.InsertLog("InsertVisit", lEmployeeID, JsonResultlist, lVisitParamlist.FirstOrDefault().BranchID, lVisitParamlist.FirstOrDefault().deviceID, sizeKB);

            return resultVisit;
        }

        public Core.Model.mdlResultList InsertLogVisit(List<Core.Model.mdlLogVisitParam> lVisitParamlist)
        {
            //string serlJson = JsonConvert.SerializeObject(lVisitParamlist);
            string lEmployeeID = lVisitParamlist.FirstOrDefault().EmployeeID;



            var resultVisit = LogVisitFacade.InsertLogVisit(lVisitParamlist);

            string JsonResultlist = JsonConvert.SerializeObject(resultVisit);
            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            var strJson = Core.Services.RestPublisher.Serialize(lVisitParamlist);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";
            Core.Manager.LogFacade.InsertLog("InsertLogVisit", lEmployeeID, JsonResultlist, lVisitParamlist.FirstOrDefault().BranchID, lVisitParamlist.FirstOrDefault().deviceID, sizeKB);

            return resultVisit;
        }

        public Core.Model.mdlResultList InsertVisitDetail(List<Core.Model.mdlVisitDetailParamNew> lVisitDetailParamlist)
        {
            //string serlJson = JsonConvert.SerializeObject(lVisitDetailParamlist);
            string lEmployeeID = lVisitDetailParamlist.FirstOrDefault().EmployeeID;



            var resultVisitDetail = VisitFacade.InsertVisitDetail(lVisitDetailParamlist);

            string JsonResultlist = JsonConvert.SerializeObject(resultVisitDetail);
            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--

            var strJson = Core.Services.RestPublisher.Serialize(lVisitDetailParamlist);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";
            Core.Manager.LogFacade.InsertLog("InsertVisitDetail", lEmployeeID, JsonResultlist, lVisitDetailParamlist.FirstOrDefault().BranchID, lVisitDetailParamlist.FirstOrDefault().deviceID, sizeKB);

            return resultVisitDetail;
        }

        public Core.Model.mdlResultList InsertDailyCost(List<Core.Model.mdlDailyCostParam> lParamlist)
        {
            string serlJson = JsonConvert.SerializeObject(lParamlist);
            string lEmployeeID = "";
            var resultDailyCost = new Core.Model.mdlResultList();

            if (lParamlist.Count > 0)
            {
                lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;



                resultDailyCost = CostFacade.InsertDailyCost(lParamlist);

                string JsonResultlist = JsonConvert.SerializeObject(resultDailyCost);
                //--009
                //JsonResultlist = JsonResultlist.Substring(0, 500);
                //009--

                var strJson = Core.Services.RestPublisher.Serialize(lParamlist);
                var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
                string sizeKB = size.ToString() + " KB";
                Core.Manager.LogFacade.InsertLog("InsertDailyCost", lEmployeeID, JsonResultlist, lParamlist.FirstOrDefault().BranchID, lParamlist.FirstOrDefault().deviceID, sizeKB);
            }
            else
            {
                var mdlResultList = new List<Core.Model.mdlResult>();

                var mdlResult = new Core.Model.mdlResult();

                mdlResult.Result = "0";
                mdlResultList.Add(mdlResult);
                resultDailyCost.ResultList = mdlResultList;
            }
            return resultDailyCost;
        }

        public Core.Model.mdlResultList InsertTracking(Core.Model.mdlTrackingParam lParam)
        {
            //string serlJson = JsonConvert.SerializeObject(lParam);
            //string lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;

            //string result = Core.Manager.LogFacade.InsertLog("InsertTracking", serlJson, lParam.EmployeeID, "");
            var resultInsertTracking = TrackingFacade.InsertTracking(lParam);

            //update christopher
            var mdlIdleCounter = IdleCounterFacade.CheckIfIdleCounterExist(lParam.EmployeeID, lParam.BranchID);
            if (mdlIdleCounter.EmployeeID == "" || mdlIdleCounter.EmployeeID == null)
            {
                string resultInsertIdleCounter = IdleCounterFacade.InsertIdleCounter(lParam.EmployeeID, lParam.BranchID, lParam.Latitude, lParam.Longitude, Convert.ToDateTime(lParam.TrackingDate));
            }
            else
            {

                double idleRadius = 0;
                double idleTime = 0;

                List<Core.Model.mdlSettings> listSettings = GeneralSettingsFacade.GetCurrentSettings(lParam.BranchID);
                if (listSettings.Count > 0)
                {
                    foreach (var setting in listSettings)
                    {
                        if (setting.name == "IDLERADIUS")
                        {
                            idleRadius = Convert.ToDouble(setting.value);
                        }
                        else if (setting.name == "IDLETIME")
                        {
                            idleTime = Convert.ToDouble(setting.value);
                        }

                    }
                }

                double baseLatitude = double.Parse(mdlIdleCounter.Latitude, CultureInfo.InvariantCulture);
                double baseLongitude = double.Parse(mdlIdleCounter.Longitude, CultureInfo.InvariantCulture);

                double newLatitude = double.Parse(lParam.Latitude, CultureInfo.InvariantCulture);
                double newLongitude = double.Parse(lParam.Longitude, CultureInfo.InvariantCulture);
                var baseCoor = new GeoCoordinate(baseLatitude, baseLongitude);
                var newCoor = new GeoCoordinate(newLatitude, newLongitude);

                double distance = RadiusFacade.getDistance(baseCoor, newCoor);

                Core.Model.mdlLogIdleParam mdlLogIdle = new Core.Model.mdlLogIdleParam();
                mdlLogIdle.EmployeeID = lParam.EmployeeID;
                mdlLogIdle.BranchID = lParam.BranchID;
                mdlLogIdle.Longitude = lParam.Longitude;
                mdlLogIdle.Latitude = lParam.Latitude;
                mdlLogIdle.StartIdle = mdlIdleCounter.StartDate;
                
                mdlLogIdle.Now = lParam.TrackingDate; // fernandes

                TimeSpan duration = Convert.ToDateTime(mdlLogIdle.Now) - Convert.ToDateTime(mdlLogIdle.StartIdle); // fernandes
                //TimeSpan duration = DateTime.Now.Subtract(Convert.ToDateTime(mdlIdleCounter.StartDate));
                //mdlLogIdle.Duration = duration.ToString(@"hh\:mm\:ss");
                mdlLogIdle.Duration = duration.ToString();
                
                mdlLogIdle.Location = "";
                //mdlLogIdle.Location = ReverseGeocodingFacade.GetStreetName(lParam.Latitude, lParam.Longitude);

                string resultLogIdle = "";

                if (lParam.FlagCheckIn == "True")
                {
                    string resultUpdateBaseCounter = IdleCounterFacade.UpdateBaseIdleCounter(lParam.EmployeeID, lParam.BranchID, lParam.Latitude, lParam.Longitude, Convert.ToDateTime(lParam.TrackingDate));

                    mdlLogIdle.Status = "True";
                    resultLogIdle = LogIdleFacade.UpdateLogIdleClose(mdlLogIdle);
                }
                else
                {
                    if (distance > idleRadius)
                    {
                        //tidak idle

                        mdlLogIdle.Status = "True";
                        resultLogIdle = LogIdleFacade.UpdateLogIdleClose(mdlLogIdle);
                        string resultUpdateBaseCounter = IdleCounterFacade.UpdateBaseIdleCounter(lParam.EmployeeID, lParam.BranchID, lParam.Latitude, lParam.Longitude, Convert.ToDateTime(lParam.TrackingDate));
                    }
                    else
                    {
                        // idle
                        int newCounter = mdlIdleCounter.Counter + 1;
                        string resultUpdateCounter = IdleCounterFacade.UpdateIdleCounter(lParam.EmployeeID, lParam.BranchID, lParam.Latitude, lParam.Longitude, newCounter);

                        var mdlMobileConfig = IdleCounterFacade.GetMobileConfigIdleCounter(lParam.BranchID);
                        if (mdlMobileConfig.ID != null || mdlMobileConfig.ID != "")
                        {
                            double roundMaxCounter = Math.Ceiling(idleTime / Convert.ToDouble(mdlMobileConfig.Value));
                            if (newCounter >= Convert.ToInt32(roundMaxCounter))
                            {
                                string resultInsertIdleLog = IdleCounterFacade.InsertIdleLog(lParam.EmployeeID, lParam.BranchID, "", Convert.ToDateTime(lParam.TrackingDate));

                                mdlLogIdle.Status = "False";
                                var logIdle = LogIdleFacade.GetLogIdle(mdlLogIdle);
                                if (logIdle.BranchID != null)
                                {
                                    resultLogIdle = LogIdleFacade.UpdateLogIdle(mdlLogIdle);
                                }
                                else
                                {

                                    resultLogIdle = LogIdleFacade.InsertLogIdle(mdlLogIdle);
                                }
                            }
                        }

                    }
                }




            }





            //string JsonResultlist = JsonConvert.SerializeObject(resultInsertTracking);
            //--009
            //JsonResultlist = JsonResultlist.Substring(0, 500);
            //009--
            //Core.Manager.LogFacade.InsertLog("InsertTracking", serlJson, lParam.EmployeeID, JsonResultlist);

            return resultInsertTracking;
        }

        public Core.Model.mdlResult InsertAndroidKey(Core.Model.mdlSaveAndroidKeyParam param)
        {
            return JsonFacade.InsertAndroidKey(param);
        }

        public Core.Model.mdlResultSV SetUserConfig(Core.Model.mdlSetDeviceIDParam param)
        {
            return JsonFacade.SetUserConfig(param);
        }

        public Stream GetDailyMessage(Core.Model.mdlSearchParam jsonsearch)
        {
            string json = Core.Services.RestPublisher.Serialize(DailyMessageFacadeNew.LoadDailyMessage(jsonsearch));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Core.Model.mdlResultSvc DeleteDailyMessage(Core.Model.mdlDailyMsg json)
        {
            return DailyMessageFacadeNew.DeleteDailyMessage(json);
        }

        public List<Core.Model.mdlMobileConfig> GetMobileConfig(Core.Model.mdlParam param)
        {
            return MobileConfigFacade.LoadMobileConfig(param);
        }

        public Core.Model.mdlCheckinCourierRadius CheckinCourierRadius(Core.Model.mdlCheckinCourierRadiusParam param)
        {
            return JsonFacade.CheckinCourierRadius(param);
        }

        public Core.Model.mdlResult PushNotificationConfirmation(Core.Model.mdlPushNotificationConfirmationParam param)
        {
            return JsonFacade.PushNotificationConfirmation(param);
        }

        public Core.Model.mdlResultList UploadJson(Core.Model.mdlUploadJsonParam lParamlist)
        {
            return Core.Manager.JsonFacade.UploadJson(lParamlist);
        }

        public Core.Model.mdlResult CheckVersion(Core.Model.mdlParam param)
        {
            return Core.Manager.MobileConfigFacade.CheckVersion(param);
        }

        public Core.Model.mdlVehicleList LoadVehicleByBranch(Core.Model.mdlVehicleBranchParam param)
        {
            return Core.Manager.VehicleFacade.LoadVehicleByBranch(param);
        }

        public Core.Model.mdlResultList InsertCostVisit(List<Core.Model.mdlCostVisit> lParamlist)
        {
            string serlJson = JsonConvert.SerializeObject(lParamlist);
            string lEmployeeID = "";
            var resultDailyCost = new Core.Model.mdlResultList();

            if (lParamlist.Count > 0)
            {
                lEmployeeID = lParamlist.FirstOrDefault().EmployeeID;



                resultDailyCost = CostFacade.InsertCostVisit(lParamlist);

                string JsonResultlist = JsonConvert.SerializeObject(resultDailyCost);
                //--009
                //JsonResultlist = JsonResultlist.Substring(0, 500);
                //009--
                var strJson = Core.Services.RestPublisher.Serialize(lParamlist);
                var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
                string sizeKB = size.ToString() + " KB";
                Core.Manager.LogFacade.InsertLog("InsertCostVisit", lEmployeeID, JsonResultlist, lParamlist.FirstOrDefault().BranchID, lParamlist.FirstOrDefault().deviceID, sizeKB);
            }
            else
            {
                var mdlResultList = new List<Core.Model.mdlResult>();

                var mdlResult = new Core.Model.mdlResult();

                mdlResult.Result = "0";
                mdlResultList.Add(mdlResult);
                resultDailyCost.ResultList = mdlResultList;
            }
            return resultDailyCost;
        }

        public Core.Model.mdlResultList InsertBBMRatio(List<Core.Model.mdlBBMRatioParam> lBBMRatioParamlist)
        {
            string lEmployeeID = lBBMRatioParamlist.FirstOrDefault().EmployeeID;

            var result = BBMFacade.InsertBBMRatio(lBBMRatioParamlist);

            string JsonResultlist = JsonConvert.SerializeObject(result);

            var strJson = Core.Services.RestPublisher.Serialize(lBBMRatioParamlist);
            var size = System.Text.ASCIIEncoding.Unicode.GetByteCount(strJson) / 1024;
            string sizeKB = size.ToString() + " KB";
            Core.Manager.LogFacade.InsertLog("InsertRatioBBM", lEmployeeID, JsonResultlist, lBBMRatioParamlist.FirstOrDefault().BranchID, lBBMRatioParamlist.FirstOrDefault().DeviceID, sizeKB);

            return result;
        }

        public Stream GetLogin(Core.Model.mdlLoginParam param)
        {
            var listMenu = Core.Manager.MenuFacade.LoadMenuMobile(param.Role);

            var result = new Core.Model.mdlLoginMenu();

            var mdlResult = new Core.Model.mdlResultSvc();
            mdlResult.Title = "Login";
            mdlResult.StatusCode = "01";
            mdlResult.StatusMessage = "";
            mdlResult.Value = listMenu;

            string json = Core.Services.RestPublisher.Serialize(mdlResult);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;



        }

        public Stream GenerateCustID()
        {
            //var listMenu = CustomerFacade.GenerateCustomerId();
            var listMenu = CustomerFacade.GenerateCustomerIDByLastID();

            var result = new Core.Model.mdlLoginMenu();

            var mdlResult = new Core.Model.mdlResultSvc();
            mdlResult.Title = "GenereateID";
            mdlResult.StatusCode = "01";
            mdlResult.StatusMessage = "";
            mdlResult.Value = listMenu;

            string json = Core.Services.RestPublisher.Serialize(mdlResult);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GenerateBranchID()
        {
            var listMenu = BranchFacade.GenerateBranchId();

            var result = new Core.Model.mdlLoginMenu();

            var mdlResult = new Core.Model.mdlResultSvc();
            mdlResult.Title = "GenereateID";
            mdlResult.StatusCode = "01";
            mdlResult.StatusMessage = "";
            mdlResult.Value = listMenu;

            string json = Core.Services.RestPublisher.Serialize(mdlResult);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }


        //------------------------------------------------- Webbase ------------------------------------------------------//

        public Stream GetSlcBranch(Core.Model.mdlArea param)
        {
            string json = Core.Services.RestPublisher.Serialize(BranchFacade.LoadBranch(param.UserID,param.AreaID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcCustomerType()
        {
            string json = Core.Services.RestPublisher.Serialize(CustomerTypeFacade.LoadCustomerType());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcEmployeeType()
        {
            string json = Core.Services.RestPublisher.Serialize(EmployeeTypeFacade.LoadEmployeeType2());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcSupervisor()
        {
            string json = Core.Services.RestPublisher.Serialize(EmployeeFacade.LoadSupervisor2());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcRayon(Core.Model.mdlRayon param)
        {
            string json = Core.Services.RestPublisher.Serialize(RayonFacade.LoadRayon(param.UserID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcRole()
        {
            string json = Core.Services.RestPublisher.Serialize(RoleFacade.LoadSlcRole());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcAreabyRayon(Core.Model.mdlRayon param)
        {
            string json = Core.Services.RestPublisher.Serialize(AreaFacade.LoadAreabyRayon(param.UserID,param.RayonID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcBranchbyArea(Core.Model.mdlArea param)
        {
            string json = Core.Services.RestPublisher.Serialize(BranchFacade.LoadBranch(param.UserID,param.AreaID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetSlcEmployee(Core.Model.mdlBranchParam param)
        {
            string json = Core.Services.RestPublisher.Serialize(EmployeeFacade.getSlcEmployee(param.BranchID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetEmployee()
        {
            string json = Core.Services.RestPublisher.Serialize(EmployeeFacade.LoadEmployee2());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetCustomer(Core.Model.mdlCustomerParam param)
        {
            string json = Core.Services.RestPublisher.Serialize(CustomerFacade.LoadCustomer(param));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetBranch()
        {
            string json = Core.Services.RestPublisher.Serialize(BranchFacade.GetBranch());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetUserMng()
        {
            string json = Core.Services.RestPublisher.Serialize(UserFacade.GetUser());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetUserMngbyID(Core.Model.mdlUser lParam)
        {
            string json = Core.Services.RestPublisher.Serialize(UserFacade.GetUserID(lParam.UserID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }


        public Stream GetSlcCustomer(Core.Model.mdlBranchParam param)
        {
            string json = Core.Services.RestPublisher.Serialize(CustomerFacade.getSlcCustomerbyBranchID(param.BranchID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetBranchbyID(Core.Model.mdlBranchParam param)
        {
            string json = Core.Services.RestPublisher.Serialize(BranchFacade.LoadBranchByID2(param.BranchID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetUserConfig(Core.Model.mdlBranchParam param)
        {
            string json = Core.Services.RestPublisher.Serialize(UserConfigFacade.LoadUserConfig(param.BranchID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetUserLicence(Core.Model.mdlLicenceKey mdlLicenceKey)
        {
            string json = Core.Services.RestPublisher.Serialize(CustomerFacade.LoadCustomerLicenseKey(mdlLicenceKey.LicenceType));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public bool CheckAvailability(Core.Model.mdlCheckAvailability param)
        {
            return UserFacade.CheckAvailability(param);
        }

        public Core.Model.mdlResultSvc InsertUserConfig(Core.Model.mdlUserConfig param)
        {
            return UserConfigFacade.InsertUserConfig(param.DeviceID, param.BranchID, param.BranchName, param.EmployeeID, param.IpLocal, param.PortLocal, param.IpPublic, param.PortPublic, param.IpAlternative, param.PortAlternative, param.Password, param.User);
        }

        public Core.Model.mdlResultSvc UpdateUserConfig(Core.Model.mdlUserConfig param)
        {
            return UserConfigFacade.UpdateUserConfig(param.DeviceID, param.BranchID, param.BranchName, param.EmployeeID, param.IpLocal, param.PortLocal, param.IpPublic, param.PortPublic, param.IpAlternative, param.PortAlternative, param.Password, param.User,param.Uid);
        }

        public Core.Model.mdlResultSvc DeleteUserConfig(Core.Model.mdlUserConfig param)
        {
            return UserConfigFacade.DeleteUserConfig(param.DeviceID, param.Uid, param.EmployeeID);
        }

        public Core.Model.mdlResultSvc DeleteRole(Core.Model.mdlRole param)
        {
            return RoleFacade.DeleteUserRole(param.RoleID);
        }

        public Core.Model.mdlResultSvc InsertCallplan(List<Core.Model.mdlCallPlanParam> CallplanParamList)
        {
            return CallPlanFacade.InsertCallplan(CallplanParamList);
        }

        public Stream GetCallPlan(Core.Model.mdlUser2 lParam)
        {
            string json = Core.Services.RestPublisher.Serialize(CallPlanFacade.GetCallPlan(lParam.UserID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetCallPlanDetail(Core.Model.mdlCallPlanParam param)
        {
            string json = Core.Services.RestPublisher.Serialize(CallPlanFacade.GetCallPlanDetail(param.CallPlanID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Core.Model.mdlResultSvc DeleteCallPlan(Core.Model.mdlCallPlanParam param)
        {
            return CallPlanFacade.DeleteCallplan(param.CallPlanID);
        }

        public Core.Model.mdlResultSvc UpdateCPTime(Core.Model.mdlUpdateCPTimeParam param)
        {
            return CallPlanFacade.UpdateCPTime(param);
        }

        public Core.Model.mdlResultSvc UploadEmployee(Core.Model.mdlEmployeeParam param)
        {
            return EmployeeFacade.UploadEmployee(param);
        }

        public Core.Model.mdlResultSvc DeleteEmployee(Core.Model.mdlEmployeeParam param)
        {
            return EmployeeFacade.DeleteEmployee(param.EmployeeID);
        }

        public Core.Model.mdlResultSvc UploadUserMng(Core.Model.mdlUser2 param)
        {
            return UserFacade.UploadUserMng(param);
        }

        public Core.Model.mdlResultSvc DeleteUserMng(Core.Model.mdlUser param)
        {
            return UserFacade.DeleteUserMng(param.UserID);
        }

        public Core.Model.mdlResultSvc DeleteRegCustomer(Core.Model.mdlResultSurvey param)
        {
            return ResultSurveyFacade.DeleteRegCustomer(param.SurveyId);
        }

        
        public Core.Model.mdlResultSvc ExportRegCustomer(Core.Model.mdlResultSurvey param)
        {
            return ResultSurveyFacade.ExportRegCustomer(param.SurveyId, param.CustType);
        }

        public Core.Model.mdlResultSvc DeleteCustomer(Core.Model.mdlCustomerParam param)
        {
            return CustomerFacade.DeleteCustomer(param.CustomerID);
        }

        public Core.Model.mdlResultSvc DeleteBranch(Core.Model.mdlBranch param)
        {
            return BranchFacade.DeleteBranch2(param.BranchID);
        }

        public Core.Model.mdlResultSvc UploadCustomer(Core.Model.mdlCustomerParam param)
        {
            return CustomerFacade.UploadCustomer(param);
        }
        public Core.Model.mdlResultSvc UploadBranch(Core.Model.mdlBranch BranchPrm)
        {
            return BranchFacade.UploadBranch(BranchPrm);
        }

        public Stream ShowSurveyCust()
        {
            string json = Core.Services.RestPublisher.Serialize(ResultSurveyFacade.LoadSurveyCustomer(DateTime.Parse("2017-12-01"), DateTime.Now));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Stream GetRayon()
        {
            string json = Core.Services.RestPublisher.Serialize(RayonFacade.LoadRayon());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Core.Model.mdlResultSvc DeleteRayon(Core.Model.mdlRayon param)
        {
            return RayonFacade.DeleteRayon(param.RayonID);
        }

        public Core.Model.mdlResultSvc UploadRayon(Core.Model.mdlRayon RayonPrm)
        {
            return RayonFacade.UploadRayon(RayonPrm);
        }

        public Stream GetArea()
        {
            string json = Core.Services.RestPublisher.Serialize(AreaFacade.LoadArea());
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

        public Core.Model.mdlResultSvc DeleteArea(Core.Model.mdlArea param)
        {
            return AreaFacade.DeleteArea(param.AreaID);
        }

        public Core.Model.mdlResultSvc UploadArea(Core.Model.mdlArea AreaPrm)
        {
            return AreaFacade.UploadArea(AreaPrm);
        }

        public Stream GetSlcBranchByUserID(string UserID)
        {
            string json = Core.Services.RestPublisher.Serialize(UserManagementFacade.LoadBranchByUserID(UserID));
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return ms;
        }

    }
}
