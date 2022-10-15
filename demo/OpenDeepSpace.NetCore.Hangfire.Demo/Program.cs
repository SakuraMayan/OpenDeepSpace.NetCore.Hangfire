using Hangfire;
using Hangfire.MySql;
using OpenDeepSpace.NetCore.Hangfire;
using OpenDeepSpace.NetCore.Hangfire.Demo.Jobs;
using OpenDeepSpace.NetCore.Hangfire.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Hangfire��ʹ��
builder.Services.AddHangfire(
		opt =>
		{
			opt.UseStorage(new MySqlStorage("Data Source=127.0.0.1;Initial Catalog=ods;User ID=root;Password=wy.023;Charset=utf8;Port=3306;Allow User Variables=true;", new MySqlStorageOptions()
			{

				TablesPrefix = "hangfire"
			}));

			//ע��ʹ����RecurringJobAttribute���Ե�������Job
			opt.RegisterRecurringJobs();

		}
	);

//���HangfireServer
builder.Services.AddHangfireServer(opt =>
{
    //���Hangfire���������

    opt.Queues =new[] { "default", "local", "recurringjobqueue" };//����һ��Ҫָ�� ���Jobָ���˶��� ����û���� Job���޷�ִ�� ��������ָ��ǰ�������м�default local

});

//ע�������Job
builder.Services.RegisterParametricJobs();
//���JobState״̬��� ���ڳɹ���ʧ��ִ�н���Ĵ���
GlobalJobFilters.Filters.Add(new JobStateFilter(builder.Services));
//ע��Job�ɹ�ʧ�ܵĴ���ʵ��
builder.Services.AddTransient<IJobExecuteResultHandler, JobExecuteResultHandler>();
//���óɹ�ִ�е�Job�־û�ʱ��
GlobalStateHandlers.Handlers.Add(new SucceededJobExpiredHandler());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//ʹ�ÿ������
app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

app.Run();
