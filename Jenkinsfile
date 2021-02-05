pipeline{
	agent any
		triggers {
		 cron("0 * * * *")
}
stages {
	stage("Build") {
	  steps {
	    sh "dotnet build CarDealerWebApi/CarShopWebApp.csproj"
		}
}
	stage("Test") {
	  steps {
	    sh "dotnet build CarDealerWebApi/CarShopWebApp.csproj"
		}
	}
}
}
	