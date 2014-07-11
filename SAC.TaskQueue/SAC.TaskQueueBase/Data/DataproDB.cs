using System;
using System.Data.Entity;
using SAC.TaskQueue.Data;
using SAC.TaskQueue.Data.Model;

namespace SAC.TaskQueue.Data
{
    public class DataproDB : DbContext
    {
               //change EF's default db to datapro & turn off migrations
        public DataproDB() :base("DataPro") {
            //Disable initializer
            Database.SetInitializer<DataproDB>(null);
        }

        public DbSet<Model.TaskQueue> TaskQueue { get; set; }
        public DbSet<Model.Task> Tasks { get; set; }


        #region Mappings
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.TaskQueue>().ToTable("TaskQueue", "app");
            modelBuilder.Entity<Model.TaskQueue>().HasKey(t => t.Id);
            modelBuilder.Entity<Model.TaskQueue>().Property(t => t.Id).HasColumnName("TaskQueueId");

            modelBuilder.Entity<Model.Task>().ToTable("Task", "app");
            modelBuilder.Entity<Model.Task>().HasKey(t=>t.Id);
            modelBuilder.Entity<Model.Task>().Property(t => t.Id).HasColumnName("TaskId");
            modelBuilder.Entity<Model.Task>().Property(t => t.Name).HasColumnName("TaskName");
            
        }
        #endregion
    }
}
