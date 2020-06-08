
#https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/?tabs=vs

# 数据迁移
#创建迁移
Add-Migration Migrations

#更新数据库
update-database

#删除迁移
Remove-Migration

#还原迁移
Update-Database LastGoodMigration

#生成sql脚本
Script-Migration

