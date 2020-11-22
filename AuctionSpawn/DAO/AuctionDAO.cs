using AuctionSpawn.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace AuctionSpawn.DAO
{
    public class AuctionDAO : IConnectionFactory<Auction>
    {
        SqlConnection con;

        public void ConnectToDatabase()
        {
            try
            {
                String conString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
                con = new SqlConnection(conString);
                con.Open();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Encountered SQL Exception while connecting to Database!", se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured while connecting to Database!", e.Message);
            }
        }

        public void Save(string val)
        {
            ConnectToDatabase();

            string sqlQuery = "INSERT INTO dbo.Auction (Description, Date, ItemQuantity) VALUES (@AuctionDescription, @Date, @ItemQuantity)";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, con);

                //1st execute query for Auction
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@AuctionDescription", Value = val, SqlDbType = SqlDbType.NVarChar });
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@Date", Value = DateTime.Now, SqlDbType = SqlDbType.DateTime });
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@ItemQuantity", Value = Configuration.ItemQuantity, SqlDbType = SqlDbType.Int });
                int rowCount = sqlCommand.ExecuteNonQuery();
                if (rowCount > 0)
                {
                    Console.WriteLine("Auction Record has been successfully created!");
                }
                else
                {
                    Console.WriteLine("Auction Record creation failed!");
                }

            }
            catch (Exception ex)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, ex.Message, ex.InnerException);

            }
            finally
            {
                con.Close();
            }
        }

        public void AddItemToAuctionDB(int id, Item item)
        {
            ConnectToDatabase();

            string sqlQuery = "INSERT INTO dbo.Item VALUES (@AuctionID ,@ItemTitle, @ItemDescription ,@StartPrice)";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, con);
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@AuctionID", Value = id, SqlDbType = SqlDbType.Int });
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@ItemTitle", Value = item.Title, SqlDbType = SqlDbType.NVarChar });
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@ItemDescription", Value = item.Description, SqlDbType = SqlDbType.NVarChar });
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@StartPrice", Value = item.StartPrice, SqlDbType = SqlDbType.Decimal });
                int rowCount = sqlCommand.ExecuteNonQuery();
                if (rowCount > 0)
                {
                    Console.WriteLine("Record has been successfully created!");
                }
                else
                {
                    Console.WriteLine("Record creation failed!");
                }

            }
            catch (SqlException se)
            {
                Console.WriteLine("Encountered SQL Exception while creating data into Database! " + se.Message);
                throw new System.Exception("Exception to AddItem \n\nException: " + se.Message);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred while creating data! " + e.Message);
                throw new System.Exception("Exception to AddItem \n\nException: " + e.Message);
            }
            finally
            {
                con.Close();
            }
        }
        
        public Auction GetById(int id)
        {
            Auction auction = new Auction();
            Item item = new Item();
            DataTable dataTable = new DataTable();

            IEnumerable<Item> items = Enumerable.Empty<Item>();

            string sqlQuery = "Select Auction.* from Auction where Auction.ID = @auctionID";

            if (id>0)
            {
                try
                {
                    ConnectToDatabase();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlQuery, con);
                    sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@auctionID",
                        Value = id,
                        SqlDbType = SqlDbType.Int
                    });
                    sqlDataAdapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            auction.ID = Convert.ToInt32(dr["ID"]);
                            auction.Description = dr["Description"].ToString();
                            auction.Date = Convert.ToDateTime(dr["Date"]);
                            auction.ItemQuantity = Convert.ToInt32(dr["ItemQuantity"]);
                        }

                        sqlDataAdapter.Dispose();
                    }
                }
                catch (SqlException se)
                {
                    Console.WriteLine("Encountered SQL Exception while populating auction details into Database!", se.Message);
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("Encountered Invalid Operation Exception while populating auction details into Database!", ioe.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception occurred while populating auction details!", e.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return auction;
        }
        
        public void DeleteById(int id)
        {
            ConnectToDatabase();

            SqlCommand sqlCommand = new SqlCommand("DELETE FROM Auction WHERE ID = @AuctionID", con);
            try
            {
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@AuctionID", Value = id, SqlDbType = SqlDbType.Int });

                int rowCount = Convert.ToInt32(sqlCommand.ExecuteNonQuery());
                if (rowCount == 1)
                {
                    Console.WriteLine("Auction " + id + " record has been successfully deleted!");
                }
                else
                {
                    Console.WriteLine("Failed to delete auction " + id + "! Please try again!");
                }
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine("Encountered SQL Exception while deleting auction!", sqlex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred while deleting auction", e.Message);
            }
            finally
            {
                con.Close();
            }
        }
        
        public IEnumerable<Auction> RetrieveAuctionsFromDB()
        {
            IEnumerable<Auction> list = Enumerable.Empty<Auction>();
            DataTable dataTable = new DataTable();
            string sqlQuery = "Select * from Auction";

            try
            {
                ConnectToDatabase();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlQuery, con);
                sqlDataAdapter.Fill(dataTable);

                        list = (from DataRow dr in dataTable.Rows
                                select new Auction()
                                {
                                    ID = dr.Field<int>("ID"),
                                    Description =  dr.Field<string>("Description"),
                                    Date = dr.Field<DateTime>("Date"),
                                    ItemQuantity = dr.Field<int>("ItemQuantity")
                                });
                
                sqlDataAdapter.Dispose();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Encountered SQL Exception while populating data into Database!", se.Message);
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine("Encountered Invalid Operation Exception while populating data into Database!", ioe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred while populating data!", e.Message);
            }
            finally
            {
                con.Close();
            }
            return list;
        } 

        public void Update(Auction auction)
        {
            ConnectToDatabase();

            DataTable dataTable = new DataTable();

            Auction newAuction = new Auction();

            string sqlQuery = "UPDATE Auction SET ItemQuantity = @NewTotal WHERE ID = @AuctionID";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, con);
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@NewTotal", Value = auction.ItemQuantity, SqlDbType = SqlDbType.Int });
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@AuctionID", Value = auction.ID, SqlDbType = SqlDbType.Int });
                sqlCommand.ExecuteNonQuery();
                

            }
            catch (Exception ex)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, ex.Message, ex.InnerException);

            }
            finally
            {
                con.Close();
            }
        }

        IEnumerable<Auction> IConnectionFactory<Auction>.RetrieveList(int id)
        {
            throw new NotImplementedException();
        }
    }
}