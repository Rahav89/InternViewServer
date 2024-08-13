namespace InternViewServer.Models.DAL
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class DBservices
    {


        //--------------------------------------------------------------------------------------------------
        // This method creates a connection to the database according to the connectionString name in the web.config 
        //--------------------------------------------------------------------------------------------------
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }
        //--------------------------------
        // This method Reads all Interns
        //--------------------------------
        public List<Intern> ReadIntern()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandWithStoredProcedure("SP_ReadAllInterns", con, null);             // create the command


            List<Intern> InternList = new List<Intern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    Intern intern = new Intern();//צריך לבצע המרות כי חוזר אובייקט
                    intern.Id = Convert.ToInt32(dataReader["Intern_id"]);//המרות של טיפוסים 
                    intern.Password_i = dataReader["Password_i"].ToString();
                    intern.First_name = dataReader["First_name"].ToString();
                    intern.Last_name = dataReader["Last_name"].ToString();
                    intern.Interns_year = dataReader["Interns_year"].ToString();
                    intern.Interns_rating = Convert.ToInt32(dataReader["Interns_rating"]);
                    intern.isManager = Convert.ToBoolean(dataReader["isManager"]);
                    intern.Email_I = dataReader["Email_I"].ToString();
                    InternList.Add(intern);
                }
                return InternList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        ////--------------------------------
        //// This method Reads all Intern Schedule
        ////--------------------------------
        //public List<InternSchedule> ReadAllInternSchedule()
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect("myProjDB"); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }


        //    cmd = CreateCommandWithStoredProcedure("SP_ReadAllInterns", con, null);             // create the command


        //    List<Intern> InternList = new List<Intern>();

        //    try
        //    {
        //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //        while (dataReader.Read())//מביאה רשומה רשומה 
        //        {
        //            Intern intern = new Intern();//צריך לבצע המרות כי חוזר אובייקט
        //            intern.Id = Convert.ToInt32(dataReader["Intern_id"]);//המרות של טיפוסים 
        //            intern.Password_i = dataReader["Password_i"].ToString();
        //            intern.First_name = dataReader["First_name"].ToString();
        //            intern.Last_name = dataReader["Last_name"].ToString();
        //            intern.Interns_year = dataReader["Interns_year"].ToString();
        //            intern.Interns_rating = Convert.ToInt32(dataReader["Interns_rating"]);
        //            intern.isManager = Convert.ToBoolean(dataReader["isManager"]);
        //            intern.Email_I = dataReader["Email_I"].ToString();
        //            InternList.Add(intern);
        //        }
        //        return InternList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }

        //    finally
        //    {
        //        if (con != null)
        //        {
        //            // close the db connection
        //            con.Close();
        //        }
        //    }

        //}


        //--------------------------------
        // This method Reads all Algorithm Weights
        //--------------------------------
        public Algorithm_Weights Read_Algorithm_Weights()
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }


            cmd = CreateCommandWithStoredProcedure("SP_Read_All_Algorithm_Weights", con, null);             // create the command
            Algorithm_Weights weights = new Algorithm_Weights();
            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    weights.Skills = Convert.ToInt32(dataReader["skills"]);//המרות של טיפוסים 
                    weights.YearWeight = Convert.ToInt32(dataReader["year_weight"]);
                    weights.YearDifficulty = Convert.ToInt32(dataReader["year_difficulty"]);
                    weights.SyllabusWeight = Convert.ToInt32(dataReader["syllabus_weight"]);
                }
                return weights;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------
        // This method add new intern
        //--------------------------------
        public bool addIntern(Intern i)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@Intern_id", i.Id);
            paramDic.Add("@Password_i", i.Password_i);
            paramDic.Add("@First_name", i.First_name);
            paramDic.Add("@Last_name", i.Last_name);
            paramDic.Add("@Interns_year", i.Interns_year);
            paramDic.Add("@Interns_rating", i.Interns_rating);
            paramDic.Add("@isManager", i.isManager);
            paramDic.Add("@Email_I", i.Email_I);

            cmd = CreateCommandWithStoredProcedure("AddIntern", con, paramDic);   // create the command
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                                                         //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return numEffected>=1;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // get intern by his ID
        //--------------------------------------------------------------------------------------------------
        public Intern GetInternByID(int internID)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internID);

            cmd = CreateCommandWithStoredProcedure("SP_getInternByID", con, paramDic); // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.HasRows == false)
                {
                    return null;
                }

                Intern intern = new Intern();

                while (dataReader.Read())
                {
                    intern.Id = Convert.ToInt32(dataReader["Intern_id"]);//המרות של טיפוסים 
                    intern.Password_i = dataReader["Password_i"].ToString();
                    intern.First_name = dataReader["First_name"].ToString();
                    intern.Last_name = dataReader["Last_name"].ToString();
                    intern.Interns_year = dataReader["Interns_year"].ToString();
                    intern.Interns_rating = Convert.ToInt32(dataReader["Interns_rating"]);
                    intern.isManager = Convert.ToBoolean(dataReader["isManager"]);
                    intern.Email_I = dataReader["Email_I"].ToString();
                }

                return intern;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //--------------------------------------------------------------------------------------------------
        // check Email Intern
        //--------------------------------------------------------------------------------------------------
        public int checkEmailIntern(string email)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Email_intern", email);

            cmd = CreateCommandWithStoredProcedure("SP_checkEmailIntern", con, paramDic); // create the command
            var returnParameter = cmd.Parameters.Add("Exists", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            try
            {
                cmd.ExecuteNonQuery(); // execute the command
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
            return (int)returnParameter.Value;
        }

        //--------------------------------------------------------------------------------------------------                                                              
        // Log in Intern                                                         
        //--------------------------------------------------------------------------------------------------
        public Intern LogInInternByIDPass(int id, string password)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", id);
            paramDic.Add("@Password_i", password);

            cmd = CreateCommandWithStoredProcedure("SP_LogIninternByIDPass", con, paramDic); // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dataReader.HasRows == false)
                {
                    return null;
                }

                Intern intern = new Intern();

                while (dataReader.Read())
                {
                    intern.Id = Convert.ToInt32(dataReader["Intern_id"]);//המרות של טיפוסים 
                    intern.Password_i = dataReader["Password_i"].ToString();
                    intern.First_name = dataReader["First_name"].ToString();
                    intern.Last_name = dataReader["Last_name"].ToString();
                    intern.Interns_year = dataReader["Interns_year"].ToString();
                    intern.Interns_rating = Convert.ToInt32(dataReader["Interns_rating"]);
                    intern.isManager = Convert.ToBoolean(dataReader["isManager"]);
                    intern.Email_I = dataReader["Email_I"].ToString();
                }

                return intern;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //--------------------------------
        // This method Reads all  Surgeries
        //--------------------------------
        //public List<Surgeries> GetAllSurgeries()
        //{

        //    SqlConnection con;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect("myProjDB"); // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }


        //    cmd = CreateCommandWithStoredProcedure("SP_ReadAllSurgeries", con, null);             // create the command


        //    List<Surgeries> SurgeriesList = new List<Surgeries>();

        //    try
        //    {
        //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        //        while (dataReader.Read())//מביאה רשומה רשומה 
        //        {
        //            Surgeries surgery = new Surgeries();//צריך לבצע המרות כי חוזר אובייקט
        //            surgery.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
        //            surgery.Case_number = Convert.ToInt32(dataReader["Case_number"]);
        //            surgery.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
        //            surgery.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
        //            surgery.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
        //            surgery.Hospital_name = dataReader["Hospital_name"].ToString();
        //            SurgeriesList.Add(surgery);
        //        }
        //        return SurgeriesList;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw (ex);
        //    }

        //    finally
        //    {
        //        if (con != null)
        //        {
        //            // close the db connection
        //            con.Close();
        //        }
        //    }

        //}
        public List<Dictionary<string, object>> GetAllSurgeriesWithProcedures()
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // Create the connection
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            cmd = CreateCommandWithStoredProcedure("SP_ReadAllSurgeriesWithProcedures", con, paramDic); // Create the command

            List<Dictionary<string, object>> surgeries = new List<Dictionary<string, object>>();
            int? lastSurgeryId = null;  // To track the last processed surgery ID
            Dictionary<string, object> currentSurgery = null;

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader
                while (dataReader.Read())
                {
                    int surgeryId = Convert.ToInt32(dataReader["Surgery_id"]);

                    // Check if this is a new surgery
                    if (lastSurgeryId == null || surgeryId != lastSurgeryId)
                    {
                        // Create a new surgery entry
                        currentSurgery = new Dictionary<string, object>
                {
                        {"Surgery_id", surgeryId},
                        {"procedureName", new List<string> { Convert.ToString(dataReader["procedureName"]) }},
                        {"Hospital_name", Convert.ToString(dataReader["Hospital_name"])},
                        {"Patient_age", Convert.ToInt32(dataReader["Patient_age"])},
                        {"Surgery_date", Convert.ToDateTime(dataReader["Surgery_date"])},
                        {"Difficulty_level", Convert.ToInt32(dataReader["Difficulty_level"])},
                        {"Case_number", Convert.ToInt32(dataReader["Case_number"])}
                };
                        surgeries.Add(currentSurgery);
                        lastSurgeryId = surgeryId; // Update the last surgery ID
                    }
                    else
                    {
                        // Append the procedure name to the existing entry's procedureName list
                        List<string> procedures = currentSurgery["procedureName"] as List<string>;
                        procedures.Add(Convert.ToString(dataReader["procedureName"]));
                    }
                }
                return surgeries;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed
                }
            }
        }


        //--------------------------------------------------------------------------------------------------
        // This method get surgery by id
        //--------------------------------------------------------------------------------------------------
        public List<Surgeries> GetSurgeriesByID(int surgeryID)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Surgery_id", surgeryID);

            cmd = CreateCommandWithStoredProcedure("SP_ReadSurgeryById", con, paramDic); // create the command


            List<Surgeries> SurgeriesList = new List<Surgeries>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.HasRows == false)
                {
                    return null;  //return null if doesnt found
                }
                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    Surgeries surgery = new Surgeries();//צריך לבצע המרות כי חוזר אובייקט
                    surgery.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
                    surgery.Case_number = Convert.ToInt32(dataReader["Case_number"]);
                    surgery.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
                    surgery.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
                    surgery.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
                    SurgeriesList.Add(surgery);
                }
                return SurgeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


   
        //--------------------------------
        // This method Reads all Procedure
        //--------------------------------
        public List<Procedure> ReadProcedure()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SP_ReadAllProcedure", con, null); // create the command

            List<Procedure> ProcedureList = new List<Procedure>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // create the object that reads from SQL

                if (!dataReader.HasRows)
                {
                    return null;  // return null if doesn't found
                }

                while (dataReader.Read()) // brings record by record
                {
                    Procedure procedure = new Procedure();
                    procedure.procedure_Id = Convert.ToInt32(dataReader["procedure_Id"]);
                    procedure.procedureName = dataReader["procedureName"].ToString();
                    procedure.category_Id = (dataReader["category_Id"] != DBNull.Value) ? Convert.ToInt32(dataReader["category_Id"]) : 0;
                    procedure.quantityAsMain = (dataReader["quantityAsMain"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsMain"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס
                    procedure.quantityAsFirst = (dataReader["quantityAsFirst"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsFirst"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס
                    procedure.quantityAsSecond = (dataReader["quantityAsSecond"] != DBNull.Value) ? Convert.ToInt32(dataReader["quantityAsSecond"]) : 0;//בדיקה אם הערך במסד הנתונים הוא ריק תשים במקומו אפס

                    ProcedureList.Add(procedure);
                }
                return ProcedureList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        //--------------------------------
        // This method Get all Procedure name
        //--------------------------------
        public List<Procedure> GetAllprocedureName()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("SP_GetProcedureNames", con, null); // create the command

            List<Procedure> ProcedureList = new List<Procedure>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // create the object that reads from SQL

                while (dataReader.Read()) // brings record by record
                {
                    Procedure procedure = new Procedure();
                    procedure.procedureName = dataReader["procedureName"].ToString();

                    ProcedureList.Add(procedure);
                }
                return ProcedureList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        //--------------------------------------------------------------------------------------------------
        // This method get All the surgeries done by the intern - for calander
        //--------------------------------------------------------------------------------------------------
        public List<Dictionary<string, object>> AllInternSurgeries(int internId)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // Create the connection
            }
            catch (Exception ex)
            {
                // Write to log
                throw ex; // Rethrow the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);

            cmd = CreateCommandWithStoredProcedure("SP_SurgeriesByInternID", con, paramDic); // Create the command

            List<Dictionary<string, object>> surgeries = new List<Dictionary<string, object>>();
            int? lastSurgeryId = null;  // To track the last processed surgery ID

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader
                while (dataReader.Read())
                {
                    int currentSurgeryId = Convert.ToInt32(dataReader["Surgery_id"]);

                    // Check if this row's surgery ID matches the last processed one
                    if (!lastSurgeryId.HasValue || lastSurgeryId.Value != currentSurgeryId)
                    {
                        // New surgery entry
                        Dictionary<string, object> surgery = new Dictionary<string, object>
                        {
                            {"Surgery_id", currentSurgeryId},
                            {"procedureName", new List<string> {Convert.ToString(dataReader["procedureName"])}},
                            {"Intern_role", Convert.ToString(dataReader["Intern_role"])},
                            {"Hospital_name", Convert.ToString(dataReader["Hospital_name"])},
                            {"Patient_age", Convert.ToInt32(dataReader["Patient_age"])},
                            {"Surgery_date", Convert.ToDateTime(dataReader["Surgery_date"])},
                            {"Difficulty_level", Convert.ToInt32(dataReader["Difficulty_level"])}
                        };
                        surgeries.Add(surgery);
                        lastSurgeryId = currentSurgeryId; // Update the last surgery ID
                    }
                    else
                    {
                        // Append the procedure name to the last entry's procedureName list
                        List<string> procedures = surgeries.Last()["procedureName"] as List<string>;
                        procedures.Add(Convert.ToString(dataReader["procedureName"]));
                    }
                }
                return surgeries;
            }
            catch (Exception ex)
            {
                // Write to log
                throw ex; // Rethrow the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed
                }
            }
        }

        ////--------------------------------------------------------------------------------------------------
        ////Update OR ADD internInSurgery - 
        ////--------------------------------------------------------------------------------------------------
        public bool UpdateOrAddInternInSurgery(InternInSurgery match)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // Create the connection
            }
            catch (Exception ex)
            {
                // Write to log
                throw ex; // Rethrow the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Surgery_id", match.Surgery_id);
            paramDic.Add("@Intern_id", match.Intern_id);
            paramDic.Add("@Intern_role", match.Intern_role);           

            cmd = CreateCommandWithStoredProcedure("SP_UpdateInternInSurgery", con, paramDic); // create the command
            var returnParameter = cmd.Parameters.Add("@Result", SqlDbType.Int); //- ערך חוזר
            returnParameter.Direction = ParameterDirection.ReturnValue;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
            return Convert.ToInt32(returnParameter.Value) == 1;
        }

        ////--------------------------------------------------------------------------------------------------
        ////get the interns of given ssurgery -FOR THE OLD ALGO, BUT CAN BE USEFUL
        ////--------------------------------------------------------------------------------------------------
        public List<Dictionary<string, object>> GetSurgeryRoles(int surgery_id)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@surgery_id", surgery_id);

            cmd = CreateCommandWithStoredProcedure("SP_GetSurgeryRoles", con, paramDic); // create the command

            List<Dictionary<string, object>> surgeries = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> surgery = new Dictionary<string, object>
                    {
                        {"Intern_role", Convert.ToString(dataReader["Intern_role"])},
                        {"First_name",Convert.ToString(dataReader["First_name"])},
                        {"Last_name", Convert.ToString(dataReader["Last_name"])},
                    };

                    surgeries.Add(surgery);
                }
                return surgeries;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }


        //--------------------------------------------------------------------------------------------------
        // This method get 5 recent surgeries done by the intern, order by date
        //--------------------------------------------------------------------------------------------------

        public List<SurgeriesOfIntern> FiveRecentInternSurgeries(int internId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);


            cmd = CreateCommandWithStoredProcedure("SP_FiveRecentInternSurgeries", con, paramDic); // create the command


            List<SurgeriesOfIntern> FiveRecentInternSurgeriesList = new List<SurgeriesOfIntern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    SurgeriesOfIntern recentSurgeriesOfIntern = new SurgeriesOfIntern();//צריך לבצע המרות כי חוזר אובייקט
                    recentSurgeriesOfIntern.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
                    recentSurgeriesOfIntern.procedureName = dataReader["procedureName"].ToString();
                    recentSurgeriesOfIntern.Intern_role = dataReader["Intern_role"].ToString();
                    recentSurgeriesOfIntern.Case_number = Convert.ToInt32(dataReader["Case_number"]);
                    recentSurgeriesOfIntern.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
                    recentSurgeriesOfIntern.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
                    recentSurgeriesOfIntern.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
                    FiveRecentInternSurgeriesList.Add(recentSurgeriesOfIntern);
                }
                return FiveRecentInternSurgeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        //--------------------------------------------------------------------------------------------------
        // This method the Syllabus of the intern
        //--------------------------------------------------------------------------------------------------
        public List<SyllabusOfIntern> GetSyllabusOfIntern(int internId)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);


            cmd = CreateCommandWithStoredProcedure("SP_SyllabusOfIntern", con, paramDic); // create the command


            List<SyllabusOfIntern> syllabusOfIntern = new List<SyllabusOfIntern>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())//מביאה רשומה רשומה 
                {

                    SyllabusOfIntern rowOfSyllabusOfIntern = new SyllabusOfIntern();//צריך לבצע המרות כי חוזר אובייקט
                    rowOfSyllabusOfIntern.procedure_Id = Convert.ToInt32(dataReader["procedure_Id"]);
                    rowOfSyllabusOfIntern.procedureName = dataReader["procedureName"].ToString();
                    rowOfSyllabusOfIntern.syllabus = Convert.ToInt32(dataReader["syllabus"]);
                    rowOfSyllabusOfIntern.haveDone = Convert.ToInt32(dataReader["haveDone"]);
                    rowOfSyllabusOfIntern.need = Convert.ToInt32(dataReader["need"]);
                    syllabusOfIntern.Add(rowOfSyllabusOfIntern);
                }
                return syllabusOfIntern;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        ////--------------------------------------------------------------------------------------------------
        ////This method update a user to the user table 
        ////--------------------------------------------------------------------------------------------------
        public int UpdateIntern(Intern intern)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@Intern_id", intern.Id);
                paramDic.Add("@Password_i", intern.Password_i);
                paramDic.Add("@First_name", intern.First_name);
                paramDic.Add("@Last_name", intern.Last_name);
                paramDic.Add("@Interns_year", intern.Interns_year);
                paramDic.Add("@Interns_rating", intern.Interns_rating);
                paramDic.Add("@isManager", intern.isManager);
                paramDic.Add("@Email_I", intern.Email_I);

                cmd = CreateCommandWithStoredProcedure("SP_UpdateUser", con, paramDic); // create the command
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected; // return the number of records affected
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // Rethrow the exception after logging it
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed in the finally block
                }
            }
        }
        ////--------------------------------------------------------------------------------------------------
        ////This method Update Algorithm Weights
        ////--------------------------------------------------------------------------------------------------
        public int Update_Algorithm_Weights(Algorithm_Weights weights)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@new_skills", weights.Skills);
                paramDic.Add("@new_year_weight", weights.YearWeight);
                paramDic.Add("@new_year_difficulty", weights.YearDifficulty);
                paramDic.Add("@new_syllabus_weight", weights.SyllabusWeight);
              
                cmd = CreateCommandWithStoredProcedure("SP_Update_Algorithm_Weights", con, paramDic); // create the command
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected; // return the number of records affected
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // Rethrow the exception after logging it
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed in the finally block
                }
            }
        }
        //----------------------------
        //sets new password for intern by email
        //----------------------------
        public int UpdateInternPassword(string email, string password)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@email", email);
                paramDic.Add("@newPass", password);

                cmd = CreateCommandWithStoredProcedure("SP_newPassIntern", con, paramDic); // create the command
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected; // return the number of records affected
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // Rethrow the exception after logging it
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed in the finally block
                }
            }
        }
        //----------------------------
        //gets all the interns and their procedure count
        //----------------------------
        public List<InternProcedureCounter> InternProcedureSummary()
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();


            cmd = CreateCommandWithStoredProcedure("SP_CountProceduresByIntern", con, paramDic); // create the command

            List<InternProcedureCounter> summaries = new List<InternProcedureCounter>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    InternProcedureCounter summary = new InternProcedureCounter();
                    summary.InternId = Convert.ToInt32(dataReader["Intern_id"]);
                    summary.FirstName = Convert.ToString(dataReader["First_name"]);
                    summary.LastName = Convert.ToString(dataReader["Last_name"]);
                    summary.InternsRating = Convert.ToInt32(dataReader["Interns_rating"]);
                    summary.InternsYear = Convert.ToString(dataReader["Interns_year"]);
                    summary.ProcedureCount = Convert.ToInt32(dataReader["ProcedureCount"]);
                    summary.OverallNeed = Convert.ToInt32(dataReader["overAllNeed"]);
                    summaries.Add(summary);
                }
                return summaries;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //----------------------------
        //gets all intern's procedure count by main/first/second
        //----------------------------
        public List<Dictionary<string, object>> FullDetailedSyllabusOfIntern(int internId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // Log the exception
                throw; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);

            cmd = CreateCommandWithStoredProcedure("SP_GetInternDetailedSyllabus", con, paramDic); // create the command

            List<Dictionary<string, object>> summaries = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> summary = new Dictionary<string, object>
            {
                        { "procedure_Id", Convert.ToInt32(dataReader["procedure_Id"]) },
                        { "procedureName", Convert.ToString(dataReader["procedureName"]) },
                        { "category_Id", Convert.ToInt32(dataReader["category_Id"]) },
                        { "categoryName", Convert.ToString(dataReader["CategoryName"]) },
                        { "requiredAsMain", Convert.ToInt32(dataReader["requiredAsMain"]) },
                        { "requiredAsFirst", Convert.ToInt32(dataReader["requiredAsFirst"]) },
                        { "requiredAsSecond", Convert.ToInt32(dataReader["requiredAsSecond"]) },
                        { "doneAsMain", Convert.ToInt32(dataReader["doneAsMain"]) },
                        { "doneAsFirst", Convert.ToInt32(dataReader["doneAsFirst"]) },
                        { "doneAsSecond", Convert.ToInt32(dataReader["doneAsSecond"]) },
                        { "categoryRequiredFirst", Convert.ToInt32(dataReader["categoryRequiredFirst"]) },
                        { "categoryRequiredSecond", Convert.ToInt32(dataReader["categoryRequiredSecond"]) }
            };
                    summaries.Add(summary);
                }
                return summaries;

            }
            catch (Exception ex)
            {
                // Log the exception
                throw; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }


        //-----------------------------------
        //Get Intern Surgeries By Procedure
        //----------------------------------
        public List<Dictionary<string, object>> GetInternSurgeriesByProcedure(int internId, int procedureID)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@InternID", internId);
            paramDic.Add("@ProcedureID", procedureID);

            cmd = CreateCommandWithStoredProcedure("SP_GetInternSurgeriesByProcedure", con, paramDic); // create the command

            List<Dictionary<string, object>> internSBPList = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> surgeryDetails = new Dictionary<string, object>
                    {
                        {"Surgery_date", Convert.ToDateTime(dataReader["Surgery_date"])},
                        {"Difficulty_level", Convert.ToInt32(dataReader["Difficulty_level"])},
                        {"Hospital_name",Convert.ToString(dataReader["Hospital_name"])},
                        {"Procedure_name", Convert.ToString(dataReader["procedureName"])},
                        {"Intern_role", Convert.ToString(dataReader["Intern_role"])}
                    };

                    internSBPList.Add(surgeryDetails);
                }
                return internSBPList;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //-----------------------------------
        //Get Intern Surgeries By Procedure name
        //----------------------------------
        public List<Dictionary<string, object>> GetInternSurgeriesByProcedureName(int internID, string procedureName)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@InternID", internID);
            paramDic.Add("@ProcedureName", procedureName);

            cmd = CreateCommandWithStoredProcedure("SP_GetInternSurgeriesByProcedureName", con, paramDic); // create the command

            List<Dictionary<string, object>> internSBPList = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> surgeryDetails = new Dictionary<string, object>
            {
                        {"Surgery_date", Convert.ToDateTime(dataReader["Surgery_date"])},
                        {"Difficulty_level", Convert.ToInt32(dataReader["Difficulty_level"])},
                        {"Hospital_name",Convert.ToString(dataReader["Hospital_name"])},
                        {"Procedure_name", Convert.ToString(dataReader["procedureName"])},
                        {"Intern_role", Convert.ToString(dataReader["Intern_role"])}
            };

                    internSBPList.Add(surgeryDetails);
                }
                return internSBPList;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }


        //-----------------------------------
        //Get all the interns exept from the one given - the ones he can talk to
        //----------------------------------
        public List<Dictionary<string, object>> GetInternsForChat(int internId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);

            cmd = CreateCommandWithStoredProcedure("SP_GetInternsForChat", con, paramDic); // create the command

            List<Dictionary<string, object>> interns = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> intern = new Dictionary<string, object>
                    {
                        {"Intern_id", Convert.ToInt32(dataReader["Intern_id"])},
                        {"First_name",Convert.ToString(dataReader["First_name"])},
                        {"Last_name", Convert.ToString(dataReader["Last_name"])},
                    };

                    interns.Add(intern);
                }
                return interns;

            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        //-----------------------------------
        //THE FOLLOWING METHODS ARE FOR THE PLACEMENT ALGORITHM
        //-----------------------------------

        //GET the surgeries that took place between the given times
        public List<Surgeries> GetSurgeriesByTime(string startDate ,  string endDate)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@startDate", startDate);
            paramDic.Add("@endDate", endDate);

            cmd = CreateCommandWithStoredProcedure("SP_SurgeriesByTime", con, paramDic); // create the command

            List<Surgeries> SurgeriesList = new List<Surgeries>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.HasRows == false)
                {
                    return null;  //return null if doesnt found
                }
                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    Surgeries surgery = new Surgeries();//צריך לבצע המרות כי חוזר אובייקט
                    surgery.Surgery_id = Convert.ToInt32(dataReader["Surgery_id"]);//המרות של טיפוסים 
                    surgery.Case_number = Convert.ToInt32(dataReader["Case_number"]);
                    surgery.Patient_age = Convert.ToInt32(dataReader["Patient_age"]);
                    surgery.Surgery_date = Convert.ToDateTime(dataReader["Surgery_date"]);
                    surgery.Difficulty_level = Convert.ToInt32(dataReader["Difficulty_level"]);
                    surgery.Hospital_name = Convert.ToString(dataReader["Hospital_name"]);
                    SurgeriesList.Add(surgery);
                }
                return SurgeriesList;
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        //GET procedures id of given surgery

        public List<int> GetProceduresOfSurgery(int SurgeryId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@SurgeryId", SurgeryId);

            cmd = CreateCommandWithStoredProcedure("SP_GetProceduresOfSurgery", con, paramDic); // create the command

            List<int> ProceduresList = new List<int>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.HasRows == false)
                {
                    return null;  //return null if doesnt found
                }
                while (dataReader.Read())//מביאה רשומה רשומה 
                {
                    ProceduresList.Add(Convert.ToInt32(dataReader["procedure_Id"]));
                }
                return ProceduresList;
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        //----------------------------
        //gets all intern's procedure count by main/first/second - FOR ALGO
        //----------------------------
        public List<Dictionary<string, object>> GetInternSyllabusForAlgo(int internId)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // create the connection
            }
            catch (Exception ex)
            {
                // Log the exception
                throw; // It's usually better to throw the original exception or log it properly
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Intern_id", internId);

            cmd = CreateCommandWithStoredProcedure("SP_GetInternSyllabusForAlgo", con, paramDic); // create the command

            List<Dictionary<string, object>> summaries = new List<Dictionary<string, object>>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader

                while (dataReader.Read())
                {
                    Dictionary<string, object> summary = new Dictionary<string, object>
                    {
                        { "procedure_Id", Convert.ToInt32(dataReader["procedure_Id"]) },
                        { "procedureName", Convert.ToString(dataReader["procedureName"]) },
                        { "category_Id", Convert.ToInt32(dataReader["category_Id"]) },
                        { "CategoryName", Convert.ToString(dataReader["CategoryName"]) },
                        { "requiredAsMain", Convert.ToInt32(dataReader["requiredAsMain"]) },
                        { "requiredAsFirst", Convert.ToInt32(dataReader["requiredAsFirst"]) },
                        { "requiredAsSecond", Convert.ToInt32(dataReader["requiredAsSecond"]) },
                        { "doneAsMain", Convert.ToInt32(dataReader["doneAsMain"]) },
                        { "doneAsFirst", Convert.ToInt32(dataReader["doneAsFirst"]) },
                        { "doneAsSecond", Convert.ToInt32(dataReader["doneAsSecond"]) },
                    };
                    summaries.Add(summary);
                }
                return summaries;

            }
            catch (Exception ex)
            {
                // Log the exception
                throw; // It's usually better to throw the original exception or log it properly
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }


        ////--------------------------------------------------------------------------------------------------
        ////This method update a Surgery to the Surgery table 
        ////--------------------------------------------------------------------------------------------------
        public int UpdateSurgeries(Surgeries surgeries)
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("@Patient_age", surgeries.Patient_age);
                paramDic.Add("@Surgery_id", surgeries.Surgery_id);
                paramDic.Add("@Difficulty_level", surgeries.Difficulty_level);
                paramDic.Add("@Case_number", surgeries.Case_number);
                paramDic.Add("@Surgery_date", surgeries.Surgery_date);
                paramDic.Add("@Hospital_name", surgeries.Hospital_name);

                cmd = CreateCommandWithStoredProcedure("SP_UpdateSurgeries", con, paramDic); // create the command
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected; // return the number of records affected
            }
            catch (Exception ex)
            {
                // write to log
                throw ex; // Rethrow the exception after logging it
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed in the finally block
                }
            }
        }
        //-----------------------------------
        //Get Future Surgeries - FOT THE OLD ALGO
        //-----------------------------------
        //public List<Dictionary<string, object>> GetFutureSurgeries()
        //{
        //    SqlConnection con = null;
        //    SqlCommand cmd;

        //    try
        //    {
        //        con = connect("myProjDB");  // create the connection
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw ex; // Rethrow the original exception or log it properly
        //    }

        //    Dictionary<string, object> paramDic = new Dictionary<string, object>();

        //    cmd = CreateCommandWithStoredProcedure("SP_FutureSurgeries", con, paramDic); // create the command

        //    List<Dictionary<string, object>> surgeries = new List<Dictionary<string, object>>();
        //    int? lastSurgeryId = null;  // To track the last processed surgery ID

        //    try
        //    {
        //        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader
        //        while (dataReader.Read())
        //        {
        //            int currentSurgeryId = Convert.ToInt32(dataReader["Surgery_id"]);

        //            // Check if this row's surgery ID matches the last processed one
        //            if (!lastSurgeryId.HasValue || lastSurgeryId.Value != currentSurgeryId)
        //            {
        //                // New surgery entry
        //                Dictionary<string, object> surgery = new Dictionary<string, object>
        //        {
        //            {"Surgery_id", currentSurgeryId},
        //            {"procedureName", Convert.ToString(dataReader["procedureName"])},
        //            {"Surgery_date", Convert.ToDateTime(dataReader["Surgery_date"])},
        //            {"Difficulty_level", Convert.ToInt32(dataReader["Difficulty_level"])},
        //            {"Patient_age", Convert.ToInt32(dataReader["Patient_age"])},
        //            {"Case_number", Convert.ToInt32(dataReader["Case_number"])},
        //            {"Hospital_name", Convert.ToString(dataReader["Hospital_name"])}
        //        };
        //                surgeries.Add(surgery);
        //                lastSurgeryId = currentSurgeryId; // Update the last surgery ID
        //            }
        //            else
        //            {
        //                // Append the procedure name to the last entry
        //                surgeries[surgeries.Count - 1]["procedureName"] += ", " + Convert.ToString(dataReader["procedureName"]);
        //            }
        //        }
        //        return surgeries;
        //    }
        //    catch (Exception ex)
        //    {
        //        // write to log
        //        throw ex; // Rethrow the original exception or log it properly
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close(); // Ensure the connection is closed
        //        }
        //    }
        //}

        public int AddSurgery(Surgeries S)
        {
            SqlConnection con;
            SqlCommand cmd;
            int result = 0; // Default result indicating failure

            try
            {
                con = connect("myProjDB"); // Create the connection
            }
            catch (Exception ex)
            {
                // Write to log
                Console.WriteLine("Connection Error: " + ex.Message);
                throw;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@Case_number", S.Case_number);
            paramDic.Add("@Patient_age", S.Patient_age);
            paramDic.Add("@Surgery_date", S.Surgery_date);
            paramDic.Add("@Difficulty_level", S.Difficulty_level);
            paramDic.Add("@Hospital_name", S.Hospital_name);

            cmd = CreateCommandWithStoredProcedure("InsertSurgery", con, paramDic); // Create the command

            // Add a parameter to capture the return value
            SqlParameter returnParameter = new SqlParameter();
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParameter);

            try
            {
                cmd.ExecuteNonQuery(); // Execute the command
                result = (int)returnParameter.Value; // Capture the return value from the stored procedure
                return result;
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
            finally
            {
                if (con != null)
                {
                    // Close the DB connection
                    con.Close();
                }
            }
        }

        public bool AddProcedureInSurgery(int surgery_id, int procedure_Id)
        {
            SqlConnection con;
            SqlCommand cmd;
            int result = 0; // Default result indicating failure

            try
            {
                con = connect("myProjDB"); // Create the connection
            }
            catch (Exception ex)
            {
                // Write to log
                Console.WriteLine("Connection Error: " + ex.Message);
                throw;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            paramDic.Add("@Surgery_id", surgery_id);
            paramDic.Add("@procedure_Id", procedure_Id);


            cmd = CreateCommandWithStoredProcedure("AddProcedureInSurgery", con, paramDic); // Create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                                                         //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return numEffected >= 1;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        
        public int AddInternsShiftsSchedule(InternSchedule IS)
        {
            SqlConnection con;
            SqlCommand cmd;
            int result = 0; // Default result indicating failure

            try
            {
                con = connect("myProjDB"); // Create the connection
            }
            catch (Exception ex)
            {
                // Write to log
                Console.WriteLine("Connection Error: " + ex.Message);
                throw;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@DutyDate", IS.DutyDate.Date);
            paramDic.Add("@Intern_id", IS.Intern_id);


            cmd = CreateCommandWithStoredProcedure("Add_Interns_shifts_Schedule", con, paramDic); // Create the command

            // Add a parameter to capture the return value
            SqlParameter returnParameter = new SqlParameter();
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(returnParameter);

            try
            {
                cmd.ExecuteNonQuery(); // Execute the command
                result = (int)returnParameter.Value; // Capture the return value from the stored procedure
                return result;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public List<InternSchedule> GetAllInternsShiftsSchedule()
        {
            SqlConnection con;
            SqlCommand cmd;


            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateCommandWithStoredProcedure("Get_All_Interns_shifts_Schedule", con, null); // create the command

            List<InternSchedule> internsDutySchedule = new List<InternSchedule>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // create the object that reads from SQL

                while (dataReader.Read()) // brings record by record
                {
                    InternSchedule internSchedule = new InternSchedule();
                    internSchedule.DutyDate = Convert.ToDateTime(dataReader["DutyDate"]);
                    internSchedule.Intern_id = Convert.ToInt32(dataReader["Intern_id"]);

                    internsDutySchedule.Add(internSchedule);
                }
                return internsDutySchedule;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }
        
        public bool DeleteInternsShiftsSchedule(InternSchedule IS)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@DutyDate", IS.DutyDate);
            paramDic.Add("@Intern_id", IS.Intern_id);

            cmd = CreateCommandWithStoredProcedure("DeleteFromInterns_shifts_Schedule", con, paramDic); // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                                                         //int numEffected = Convert.ToInt32(cmd.ExecuteScalar()); // returning the id
                return numEffected>=1;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        public List<int> GetInternsOnDutyDayBefore(DateTime GivenDate)
        {
            SqlConnection con;
            SqlCommand cmd;


            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@GivenDate", GivenDate);

            cmd = CreateCommandWithStoredProcedure("GetInternsOnDutyDayBefore", con, paramDic); // create the command


            List<int> internsIDS = new List<int>();

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // create the object that reads from SQL

                while (dataReader.Read()) // brings record by record
                {
                    int internID = Convert.ToInt32(dataReader["Intern_id"]);

                    internsIDS.Add(internID);
                }
                return internsIDS;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }



        //Delete Surgery From Surgeries Schedule
        public int DeleteSurgeryFromSurgeriesSchedule(int surgery_id)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("@Surgery_id", surgery_id);

            cmd = CreateCommandWithStoredProcedure("SP_DeleteSurgeryFromSurgeriesSchedule", con, paramDic); // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }


        public List<Dictionary<string, object>> AllSurgeriesWithInterns()
        {
            SqlConnection con = null;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB");  // Create the connection
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw ex;
            }

            Dictionary<string, object> paramDic = new Dictionary<string, object>();

            cmd = CreateCommandWithStoredProcedure("AllSurgeriesWithInterns", con, paramDic); // Create the command

            List<Dictionary<string, object>> surgeries = new List<Dictionary<string, object>>();
            int? lastSurgeryId = null;  // To track the last processed surgery ID
            Dictionary<string, object> currentSurgery = null;

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // Execute the reader
                while (dataReader.Read())
                {
                    int surgeryId = Convert.ToInt32(dataReader["Surgery_id"]);

                    // Check if this is a new surgery
                    if (lastSurgeryId == null || surgeryId != lastSurgeryId)
                    {
                        // Create a new surgery entry
                        currentSurgery = new Dictionary<string, object>
                {
                    {"Surgery_id", surgeryId},
                    {"Case_number", Convert.ToInt32(dataReader["Case_number"])},
                    {"Patient_age", Convert.ToInt32(dataReader["Patient_age"])},
                    {"Surgery_date", Convert.ToDateTime(dataReader["Surgery_date"])},
                    {"Difficulty_level", Convert.ToInt32(dataReader["Difficulty_level"])},
                    {"procedureName", new List<string> { Convert.ToString(dataReader["procedureName"]) }},
                    {"Lead_Surgeon", new Dictionary<string, object>
                        {
                            {"Id", Convert.ToInt32(dataReader["Lead_Surgeon_Id"])},
                            {"Name", Convert.ToString(dataReader["Lead_Surgeon_Name"])}
                        }
                    },
                    {"First_Assistant", new Dictionary<string, object>
                        {
                            {"Id", Convert.ToInt32(dataReader["First_Assistant_Id"])},
                            {"Name", Convert.ToString(dataReader["First_Assistant_Name"])}
                        }
                    },
                    {"Second_Assistant", new Dictionary<string, object>
                        {
                            {"Id", Convert.ToInt32(dataReader["Second_Assistant_Id"])},
                            {"Name", Convert.ToString(dataReader["Second_Assistant_Name"])}
                        }
                    }
                };

                        surgeries.Add(currentSurgery);
                        lastSurgeryId = surgeryId; // Update the last surgery ID
                    }
                    else
                    {
                        // Append the procedure name to the existing entry's procedureName list
                        List<string> procedures = currentSurgery["procedureName"] as List<string>;
                        procedures.Add(Convert.ToString(dataReader["procedureName"]));
                    }
                }
                return surgeries;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw ex;
            }
            finally
            {
                if (con != null)
                {
                    con.Close(); // Ensure the connection is closed
                }
            }
        }

        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con, Dictionary<string, object> paramDic)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            if (paramDic != null)
                foreach (KeyValuePair<string, object> param in paramDic)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                }


            return cmd;
        }
    }
}






