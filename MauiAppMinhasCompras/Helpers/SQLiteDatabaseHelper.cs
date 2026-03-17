using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        private readonly SQLiteAsyncConnection _conn;
        public SQLiteDatabaseHelper(string path) 
        { 
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public async Task<int> Insert(Produto produto) 
        {
            return await _conn.InsertAsync(produto);
        }

        public async Task<int> Update(Produto produto) 
        {
            string sql = "UPDATE Produto SET Descricao = ?, Quantidade = ?, Preco = ? WHERE Id = ?";

            return await _conn.ExecuteAsync(sql, produto.Descricao, produto.Quantidade, produto.Preco, produto.Id);
        }

        public async Task<int> Delete(int id) 
        {
            return await _conn.Table<Produto>().DeleteAsync(p => p.Id == id);
        }

        public async Task<List<Produto>> GetAll() 
        {
            return await _conn.Table<Produto>().ToListAsync();
        }

        public async Task<List<Produto>> Search(string descricao) 
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%' || ? || '%'";

            return await _conn.QueryAsync<Produto>(sql, descricao);
        }
    }
}
