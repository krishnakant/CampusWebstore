//var resultSetAjax; //Global  variable to hold result set
var fieldsAjax; //Global variable to hold post data
//var XMLHttpObject; //Global varialbe to hold XMLHttp Object
//var responseStreamAjax; //Gloabal Variable to hold data
var rowSeparator=";~"; //If passing Text Data then row Sperator
var colSeparator="~#"; //If passing Text Data then col Sperator 

function AjaxController(urls,datas,readyStateHandlers)
{
	this.url=urls;
	this.content="";
	this.content=datas;
	this.resultSet=new Array();
	this.XMLHttpObject=false;
	this.responseStream="";
	this.rowSeparator=rowSeparator;
	this.colSeparator=colSeparator;
	this.GetData=AIMLoadData; 
	this.ErrorHandler=AIMErrorHandler;
	this.ParseResult=AIMParseResultXML;
	this.readyStateHandler=readyStateHandlers;
	this.ResponseValidation=AIMResponseValidator;
	this.alternatePageLocation="";
}

//Method which send request using ajax 
function AIMLoadData() //FUN_loadData
{
		if(window.XMLHttpRequest) 
		{
    		try 
    		{
				this.XMLHttpObject = new XMLHttpRequest();
			} catch(e) {
				this.XMLHttpObject = false;
			}
			// branch for IE/Windows ActiveX version
		}
		else if(window.ActiveXObject)
		{
       		try {
        		this.XMLHttpObject = new ActiveXObject("Msxml2.XMLHTTP");
      		} catch(e) 
      		{
        		try 
        		{
          			this.XMLHttpObject = new ActiveXObject("Microsoft.XMLHTTP");
        		} catch(e) {
          			this.XMLHttpObject = false;
        		}
			}
		}
		if(this.XMLHttpObject) 
		{	
			if(this.XMLHttpObject.overrideMimeType)
				this.XMLHttpObject.overrideMimeType('text/xml');
			this.XMLHttpObject.onreadystatechange = this.readyStateHandler;
			this.XMLHttpObject.open("POST", this.url, true);
			this.XMLHttpObject.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
			this.XMLHttpObject.send(this.content);
		}
		else
		{
			this.ErrorHandler(0);
		}
}

/*Response Format Should Always in Format of
<data>..........</data>
If there is any Error Error Format Shoud be
<error>........</error>
Retun: True If there is no Error
*/
function AIMResponseValidator() //FUN_AIMResponsehandler
{
	Loading(true);
	try
	{
		if (this.XMLHttpObject.readyState == 4 && this.XMLHttpObject.status == 200) 
		{
			Loading(false);
			var dataTag=null;
			Debugger(this.responseStream);
			if(this.XMLHttpObject.responseXML)
			dataTag=this.XMLHttpObject.responseXML.getElementsByTagName("data");
			if(dataTag && dataTag.length>0 && dataTag!=null)
				{
					this.responseStream=dataTag.item(0).firstChild.data;
					return true;
				}
			else
			{	
				var errTag=null;
				if(this.XMLHttpObject.responseXML)
					this.XMLHttpObject.responseXML.getElementsByTagName("error");
				if(errTag && errTag.length>0 && errTag!=null)
					{
						this.responseStream=errTag.item(0).firstChild.data;
						this.ErrorHandler(3);
						//Debugger(responseStreamAjax);  	
					}	
				else if(this.XMLHttpObject.responseText!="")
					{
					this.responseStream=this.XMLHttpObject.responseText;
					return true;
					}
				else
					this.ErrorHandler(4);	
			}
		}				
		else if(this.XMLHttpObject.readyState == 4 && this.XMLHttpObject.status != 200)
			this.ErrorHandler(5);
	}
	catch(exception)
	{
		Debugger(exception.mesage);
		this.ErrorHandler(3);
	}
	return false;		
}


function AIMErrorHandler(statusCode)
{
	var divErrorPopUp=document.getElementById("divErrorPopUp")
	Loading(false); 
	if(!divErrorPopUp)
		{
			divErrorPopUp=document.createElement("div");
			divErrorPopUp.setAttribute("id","divErrorPopUp");
			divErrorPopUp.className="DivErrorPopUp";
			divErrorPopUp.style.position="absolute";
			divErrorPopUp.style.left="200px";
			divErrorPopUp.style.top="200px";
			document.body.appendChild(divErrorPopUp);
		}	
	if(statusCode==-1) //Hide Error Pop Up
		{
			divErrorPopUp.style.display="none";		
			return;
		}
	else
		divErrorPopUp.style.display="block";			
	switch(statusCode)
	{
		case 0: //Critical Error, not supported ajax	
			if(window.ActiveXObject) 
				divErrorPopUp.innerHTML="Please enable Activex object. <br>";
			else
				divErrorPopUp.innerHTML="Your browser does not support this functionality, Please use Internet Explorer 6.0, Mozila firefox 1.5 or higer version.";
			break;	
		case 1: //Html element Missing
			divErrorPopUp.innerHTML="Unable to process request, Please try later.";
			break;	
		case 2: //Custom error
			divErrorPopUp.innerHTML=this.responseStream;
			return;
		case 3: //UnExpected Error
			divErrorPopUp.innerHTML="Unable to process request, Please try later.";
			break;	
		case 4: //Could Not Establish Connection To server	
			divErrorPopUp.innerHTML="Unable to connect to requested server, Please check your internet connection setting.";
			break;	
		case 5: //not returned status 200
			divErrorPopUp.innerHTML="Unable to process request, Please try later.";
			break;	
	//Debugger(statusCode);  
	}
	if(this.alternateLocation!="")
		divErrorPopUp.innerHTML+="</br>If problem persist, You may click <a href='"+ this.alternateLocation +"'> here </a> to browse using alternate page.";
	divErrorPopUp.innerHTML+="<table width='100%'><tr align='center'><td><a href='javascrpt:void(0)' onClick='"+this.ErrorHandler(-1)+"'>Close</a></td></tr></table>";
	
}


function Loading(status)
{
	var divLoading;
	if(status)
	{
		divLoading=document.getElementById("divLoading");
		if(!divLoading)
			{
			divLoading=document.createElement("div");
			divLoading.setAttribute("id","divLoading");
			divLoading.setAttribute("class","LoadingState");
			//divLoading.style.backgroundColor="blue";
			//divLoading.style.color="white";
			//divLoading.style.fontSize="12pt";
			//divLoading.style.left="400px";
			//divLoading.style.top="400px";	
			///divLoading.style.position="absolute";
			}
		divLoading.style.display="block";
		divLoading.innerHTML="Loading...."
		document.body.appendChild(divLoading);
		//Debugger(divLoading.innerHTML); 		
	}
	else
	{
		divLoading=document.getElementById("divLoading");
		if(divLoading)
			divLoading.style.display="none";
	}		
}

/*---------
Parsing Function For Cahtacter sperated data
Paramenters: 
Returns : Boolean as success or fail
if sucess than it fills global resultSetAjax two dimesonal Array.
----------*/
function AIMParseResultText() //FUN_ParseResult_AIMParseResultText
{
	var response=this.responseStream;
	this.resultSet=new Array();
	var responseRowArray=new Array();
	try
	{
		responseRowArray=response.split(this.rowSeparator);
		for(i=0; i<responseRowArray.length; i++)
			this.resultSet=responseRowArray[i].split(this.colSeparator); 
	 	return true;
	}
	catch(exception)
	{
		Debuggger(exception.message);
		this.ErrorHandler(3);
	}
	return false;			 		
}

function AIMParseResultXML() //FUN_AIMParseResultXML
{
	try
	{
		this.resultSet=new Array();
		var xDoc=this.XMLHttpObject.responseXML;
		var rootNode = xDoc.getElementsByTagName("data");
		for (i=0; i<rootNode[0].childNodes.length; i++)	
		{
			this.resultSet[i]=new Array();
			innerNode=rootNode[0].childNodes[i];
			for(j=0; j<innerNode.childNodes.length;j++)
				this.resultSet[i].push(GetInnerText(innerNode.childNodes[j]));			
		}
		return true;
	}
	catch(exception)
	{
		Debugger(exception.message);
		this.ErrorHandler(3);
	}
	return false;			 		
}
function GetInnerText(node)
{
	if(node.textContent) return node.textContent;
	else  return node.text;
}
 
/*-----end ParseReult-------*/
