using AuctionSpawn.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AuctionSpawn.DAO
{
    public class ItemDAO : IConnectionFactory<Item>
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

        public void DeleteById(int itemId)
        {
            ConnectToDatabase();

            SqlCommand sqlCommand = new SqlCommand("DELETE FROM Item WHERE ItemID = @ItemId", con);
            try
            {
                sqlCommand.Parameters.Add(new SqlParameter { ParameterName = "@ItemId", Value = itemId, SqlDbType = SqlDbType.Int });

                int rowCount = Convert.ToInt32(sqlCommand.ExecuteNonQuery());
                if (rowCount == 1)
                {
                    Console.WriteLine("Item " + itemId + " record has been successfully deleted!");
                }
                else
                {
                    Console.WriteLine("Failed to delete Item " + itemId + "! Please try again!");
                }
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine("Encountered SQL Exception while deleting item!", sqlex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred while deleting item", e.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public Item GetById(int id)
        {
            ConnectToDatabase();
            DataTable dataTable = new DataTable();
            Item item = new Item();

            string sqlQuery = "SELECT * FROM Item WHERE ItemID = @ItemID";
            try
            {
                ConnectToDatabase();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlQuery, con);
                sqlDataAdapter.SelectCommand.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@ItemID",
                    Value = id,
                    SqlDbType = SqlDbType.Int
                });
                sqlDataAdapter.Fill(dataTable);


                foreach (DataRow dr in dataTable.Rows)
                {
                    item.ItemID = Convert.ToInt32(dr["ItemID"]);
                    item.AuctionID = Convert.ToInt32(dr["AuctionID"]);
                    item.Title = dr["Title"].ToString();
                    item.Description = dr["Description"].ToString();
                    item.StartPrice = Convert.ToInt32(dr["StartPrice"]);
                 }
                sqlDataAdapter.Dispose();
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine("Encountered SQL Exception while getting data!", sqlex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occurred while getting data", e.Message);
            }
            finally
            {
                con.Close();
            }
            return item;
        }

        public IEnumerable<Item> RetrieveList(int id)
        {
            DataTable dataTable = new DataTable();

            IEnumerable<Item> list = Enumerable.Empty<Item>();

            string sqlQuery = "Select * from ITEM  where ITEM.AuctionID = @auctionID";

            if (id > 0)
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

                    list = (from DataRow dr in dataTable.Rows
                            select new Item()
                            {
                                ItemID = Convert.ToInt32(dr["ItemID"]),
                                AuctionID = Convert.ToInt32(dr["AuctionID"]),
                                Title = dr["Title"].ToString(),
                                Description = dr["Description"].ToString(),
                                StartPrice = Convert.ToInt32(dr["StartPrice"])
                            });

                    sqlDataAdapter.Dispose();

                }
                catch (SqlException se)
                {
                    Console.WriteLine("Encountered SQL Exception while populating auction items into Database!", se.Message);
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("Encountered Invalid Operation Exception while auction items into Database!", ioe.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception occurred while populating auction items!", e.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return list;
        }

        public void Save(string val)
        {
            throw new NotImplementedException();
        }

        public void Update(Item obj)
        {
            throw new NotImplementedException();
        }
        
    }
}