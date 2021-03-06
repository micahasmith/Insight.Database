﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Insight.Database
{
	/// <summary>
	/// Extension methods for DbConnectionStringBuilder.
	/// </summary>
	public static class DbConnectionStringBuilderExtensions
	{
		/// <summary>
		/// Creates and returns a new DbConnection.
		/// </summary>
		/// <param name="builder">The DbConnectionStringBuilder containing the connection string.</param>
		/// <returns>A closed DbConnection.</returns>
		public static DbConnection Connection(this DbConnectionStringBuilder builder)
		{
			DbConnection connection = null;

			// get the connection from the provider
			// if the provider is not specified, then attempt to get the type
			if (builder is SqlConnectionStringBuilder)
				connection = new SqlConnection();
			else if (builder is OdbcConnectionStringBuilder)
				connection = new OdbcConnection();
			else if (builder is OleDbConnectionStringBuilder)
				connection = new OleDbConnection();

			if (connection == null)
				throw new ArgumentException("Cannot determine the type of connection from the ConnectionStringBuilder", "builder");

			connection.ConnectionString = builder.ConnectionString;
			return connection;
		}

		/// <summary>
		/// Creates and returns a new connection implementing the given interface.
		/// </summary>
		/// <typeparam name="T">The interface to implement on the connection.</typeparam>
		/// <param name="builder">The DbConnectionStringBuilder containing the connection string.</param>
		/// <returns>A closed connection that implements the given interface.</returns>
		public static T As<T>(this DbConnectionStringBuilder builder) where T : class
		{
			return builder.Connection().As<T>();
		}

		/// <summary>
		/// Opens and returns a database connection.
		/// </summary>
		/// <param name="builder">The connection string to open and return.</param>
		/// <returns>The opened connection.</returns>
		public static DbConnection Open(this DbConnectionStringBuilder builder)
		{
			return builder.Connection().OpenConnection();
		}

		/// <summary>
		/// Opens and returns a database connection.
		/// </summary>
		/// <param name="builder">The connection string to open and return.</param>
		/// <param name="cancellationToken">The cancellation token to use for the operation.</param>
		/// <returns>The opened connection.</returns>
		public static Task<DbConnection> OpenAsync(this DbConnectionStringBuilder builder, CancellationToken? cancellationToken = null)
		{
			return builder.Connection().OpenConnectionAsync(cancellationToken);
		}

		/// <summary>
		/// Opens and returns a database connection implementing a given interface.
		/// </summary>
		/// <typeparam name="T">The interface to implmement.</typeparam>
		/// <param name="builder">The connection string to open and return.</param>
		/// <returns>The opened connection.</returns>
		public static T OpenAs<T>(this DbConnectionStringBuilder builder) where T : class, IDbConnection
		{
			return builder.Connection().OpenAs<T>();
		}

		/// <summary>
		/// Asynchronously opens and returns a database connection implementing a given interface.
		/// </summary>
		/// <typeparam name="T">The interface to implmement.</typeparam>
		/// <param name="builder">The connection string to open and return.</param>
		/// <param name="cancellationToken">The cancellation token to use for the operation.</param>
		/// <returns>The opened connection.</returns>
		public static Task<T> OpenAsAsync<T>(this DbConnectionStringBuilder builder, CancellationToken? cancellationToken = null) where T : class, IDbConnection
		{
			return builder.Connection().OpenAsAsync<T>(cancellationToken);
		}

		/// <summary>
		/// Opens a database connection and begins a new transaction that is disposed when the returned object is disposed.
		/// </summary>
		/// <param name="builder">The builder for the connection.</param>
		/// <returns>A wrapper for the database connection.</returns>
		public static DbConnectionWrapper OpenWithTransaction(this DbConnectionStringBuilder builder)
		{
			return builder.Connection().OpenWithTransaction();
		}

		/// <summary>
		/// Asynchronously opens a database connection and begins a new transaction that is disposed when the returned object is disposed.
		/// </summary>
		/// <param name="builder">The builder for the connection.</param>
		/// <param name="cancellationToken">The cancellation token to use for the operation.</param>
		/// <returns>A task returning a connection when the connection has been opened.</returns>
		public static Task<DbConnectionWrapper> OpenWithTransactionAsync(this DbConnectionStringBuilder builder, CancellationToken? cancellationToken = null)
		{
			return builder.Connection().OpenWithTransactionAsync(cancellationToken);
		}

		/// <summary>
		/// Opens a database connection implementing a given interface and begins a new transaction that is disposed when the returned object is disposed.
		/// </summary>
		/// <typeparam name="T">The interface to implement.</typeparam>
		/// <param name="builder">The builder for the connection.</param>
		/// <returns>A wrapper for the database connection.</returns>
		public static T OpenWithTransactionAs<T>(this DbConnectionStringBuilder builder) where T : class, IDbConnection, IDbTransaction
		{
			return builder.Connection().OpenWithTransactionAs<T>();
		}

		/// <summary>
		/// Asynchronously opens a database connection implementing a given interface, and begins a new transaction that is disposed when the returned object is disposed.
		/// </summary>
		/// <typeparam name="T">The interface to implement.</typeparam>
		/// <param name="builder">The builder for the connection.</param>
		/// <param name="cancellationToken">The cancellation token to use for the operation.</param>
		/// <returns>A task returning a connection when the connection has been opened.</returns>
		public static Task<T> OpenWithTransactionAsAsync<T>(this DbConnectionStringBuilder builder, CancellationToken? cancellationToken = null) where T : class, IDbConnection, IDbTransaction
		{
			return builder.Connection().OpenWithTransactionAsAsync<T>(cancellationToken);
		}
	}
}
