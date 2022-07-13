using CMS.Context;
using CMS.Helper;
using CMS.Management.Model;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;
using CMS.Master.Model;

namespace CMS.API.Controllers
{
    public class CardController : APIController
    {
        [ActionName("add")]
        [HttpPost]
        public IHttpActionResult Add([FromBody] JObject json)
        {
            string nik = null;
            string carduid = null;

            try
            {
                nik = json.GetValue("nik").Value<string>().Trim();
                carduid = json.GetValue("carduid").Value<string>().Trim();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.NoContent,
                    status = "error",
                    message = "NIK or Carduid not present"
                });
            }

            var user = new M_User();
            if (isAuthorized(out user))
            {
                M_Card card = new M_Card();
                card.NIK = nik;
                card.CARDUID = carduid;
                card.CREATEBY = user.user_id;
                card.UPDATEBY = user.user_id;
                card.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                card.UPDATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
                {
                    cnn.Open();
                    using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                    {
                        card = M_Card.Insert(card, cnn, sqlTransaction);
                        Log.Insert(Log.LogType.ADD, "Add new card", JObject.FromObject(card), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                }


                return Json(new
                {
                    code = HttpStatusCode.OK,
                    status = "success",
                    message = "Add new card successfully",
                    data = card
                });
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });

        }

        [ActionName("validate")]
        [HttpPost]
        public IHttpActionResult Validate([FromBody] JObject json)
        {
            string nik = null;
            string carduid = null;

            try
            {
                nik = json.GetValue("nik").Value<string>().Trim();
                carduid = json.GetValue("carduid").Value<string>().Trim();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.NoContent,
                    status = "error",
                    message = "NIK or Carduid not present"
                });
            }

            var user = new M_User();
            if (isAuthorized(out user))
            {
                string lastUpdate = null;
                M_Card.CardStatus result = M_Card.CardStatus.NotFound;
                M_Card card = new M_Card();
                card.NIK = nik;
                card.CARDUID = carduid;

                using (var cnn = new OracleConnection(Database.getConnectionString("Default")))
                {
                    cnn.Open();
                    using (OracleTransaction sqlTransaction = cnn.BeginTransaction())
                    {
                        result = M_Card.Validate(card, cnn, sqlTransaction, out lastUpdate);
                        Log.Insert(Log.LogType.ADD, $"Check NIK {card.NIK}", JObject.FromObject(card), cnn, sqlTransaction);
                        sqlTransaction.Commit();
                    }
                }

                if (result == M_Card.CardStatus.Valid)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.OK,
                        status = "valid",
                        message = $"NIK '{card.NIK}' with Card UID '{card.CARDUID}' is valid with last update: '{lastUpdate}'",
                    });
                }
                else if (result == M_Card.CardStatus.Invalid)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.NoContent,
                        status = "invalid",
                        message = $"NIK '{card.NIK}' with Card UID '{card.CARDUID}' is not valid",
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = HttpStatusCode.NotFound,
                        status = "not_found",
                        message = $"NIK '{card.NIK}' with Card UID '{card.CARDUID}' is not found on system",
                    });
                }
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });

        }

        //Penambahan service dashboard CMS

        [HttpGet]
        public IHttpActionResult GetTotalMasal()
        {
            var summary = new M_SUMMARY_CARD();
            try
            {
                summary = summary.GetTotalMassal();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.NoContent,
                    status = "error",
                    message = ""
                });
            }

            var user = new M_User();
            if (isAuthorized(out user))
            {
                string lastUpdate = null;
               
                if (summary != null)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.OK,
                        status = "ok",
                        message = "success",
                        data = summary
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = HttpStatusCode.NotFound,
                        status = "error",
                        message = "Data Not Found",
                    });
                }
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });
        }

        [HttpGet]
        public IHttpActionResult GetTotalPiak()
        {
            var summary = new M_SUMMARY_CARD();
            try
            {
                summary = summary.GetTotalPiak();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.NoContent,
                    status = "error",
                    message = ""
                });
            }

            var user = new M_User();
            if (isAuthorized(out user))
            {
                string lastUpdate = null;

                if (summary != null)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.OK,
                        status = "ok",
                        message = "success",
                        data = summary
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = HttpStatusCode.NotFound,
                        status = "error",
                        message = "Data Not Found",
                    });
                }
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });
        }

        [HttpGet]
        public IHttpActionResult GetTotalRegisteredCard()
        {
            var summary = new M_SUMMARY_CARD();
            try
            {
                summary = summary.GetTotalRegisteredCard();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.NoContent,
                    status = "error",
                    message = ""
                });
            }

            var user = new M_User();
            if (isAuthorized(out user))
            {
                string lastUpdate = null;

                if (summary != null)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.OK,
                        status = "ok",
                        message = "success",
                        data = summary
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = HttpStatusCode.NotFound,
                        status = "error",
                        message = "Data Not Found",
                    });
                }
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });
        }

        [HttpGet]
        public IHttpActionResult GetTotalRegisteredNIK()
        {
            var summary = new M_SUMMARY_CARD();
            try
            {
                summary = summary.GetTotalRegisteredCard();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.NoContent,
                    status = "error",
                    message = ""
                });
            }

            var user = new M_User();
            if (isAuthorized(out user))
            {
                string lastUpdate = null;

                if (summary != null)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.OK,
                        status = "ok",
                        message = "success",
                        data = summary
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = HttpStatusCode.NotFound,
                        status = "error",
                        message = "Data Not Found",
                    });
                }
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });
        }

        [HttpGet]
        public IHttpActionResult GetTotalCardManufactured()
        {
            var summary = new M_SUMMARY_CARD();
            try
            {
                summary = summary.GetTotalCardManufactured();
            }
            catch
            {
                return Json(new
                {
                    code = HttpStatusCode.NoContent,
                    status = "error",
                    message = ""
                });
            }

            var user = new M_User();
            if (isAuthorized(out user))
            {
                string lastUpdate = null;

                if (summary != null)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.OK,
                        status = "ok",
                        message = "success",
                        data = summary
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = HttpStatusCode.NotFound,
                        status = "error",
                        message = "Data Not Found",
                    });
                }
            }

            return Content(HttpStatusCode.Forbidden, new
            {
                code = HttpStatusCode.Forbidden,
                status = "error",
                message = "Bearer token not valid"
            });
        }
    }
    
}