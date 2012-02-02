#include "stdafx.h"
#include <stdlib.h>
#include <string>
#include <iostream>
#include <stdio.h>
#include <sstream>
#include <algorithm>
#include <iterator>
#include <vector>
#include <set>
#include <unordered_set>
#include <direct.h>
#include <assert.h>
#include <fstream>

using namespace std;

string exec(char* cmd) {
    
	FILE* pipe = _popen(cmd, "r");
    
	if (!pipe) return "ERROR";
    
	char buffer[300];
    string result = "";
    
	while(!feof(pipe)) {
        if(fgets(buffer, 300, pipe) != NULL)
                result += buffer;
    }

    _pclose(pipe);
    return result;
}

string custom_replace(string text, string searchString, string replaceString)
{
    assert( searchString != replaceString );

    string::size_type pos = 0;

    while ( (pos = text.find(searchString, pos)) != string::npos ) 
	{
        text.replace( pos, searchString.size(), replaceString );
        pos++;
    }

	return text;
}

std::vector<std::string> split(const std::string &s, char delim) {
    
	std::vector<std::string> elems;

	std::stringstream ss(s);
    std::string item;
    while(std::getline(ss, item, delim)) {
        elems.push_back(item);
    }
    return elems;
}

const char* node_types[] =
{
    "null", "document", "element", "pcdata", "cdata", "comment", "pi", "declaration"
};

struct simple_walker: pugi::xml_tree_walker
{
	string _node;
	string _value;
	bool _append;

	simple_walker(string node,string value,bool append) : _node(node), _value(value), _append(append)
	{
	}

    virtual bool for_each(pugi::xml_node& node)
    {
		if (string(node.name()) == _node)
		{
		   pugi::xml_node child = node.first_child();

		   string new_value="";
		   if (_append)
		   new_value = child.value() + _value;
		   else new_value = _value;
			   
		   child.set_value(new_value.c_str());
		}

        return true; // continue traversal
    }
};

struct replace_value_walker: pugi::xml_tree_walker
{
	string _node;
	string _search_value;
	string _new_value;

	replace_value_walker(string node,string search_value,string new_value) : _node(node), _search_value(search_value), _new_value(new_value)
	{
	}

    virtual bool for_each(pugi::xml_node& node)
    {
		if (string(node.name()) == _node)
		{
		   pugi::xml_node child = node.first_child();

		   string v = custom_replace(string(child.value()),_search_value,_new_value);
			   
		   child.set_value(v.c_str());
		}

        return true; // continue traversal
    }
};

string GetGlobalIncludes(string openvibe_base)
{
	vector<string> incl;

	incl.push_back(openvibe_base+"/openvibe/trunc/include");
    incl.push_back(openvibe_base+"/openvibe-toolkit/trunc/src");
    incl.push_back(openvibe_base+"/cmake-modules");
    incl.push_back(openvibe_base+"/openvibe-modules/ebml/trunc/include");
    incl.push_back(openvibe_base+"/openvibe-toolkit/trunc/include/openvibe-toolkit/tools");
    incl.push_back(openvibe_base+"/openvibe-modules/system/trunc/include");
    incl.push_back(openvibe_base+"/openvibe-modules/xml/trunc/include");

	incl.push_back(openvibe_base+"/openvibe-modules/fs/trunc/include");
	incl.push_back(openvibe_base+"/openvibe-modules/automaton/trunc/include");
	incl.push_back(openvibe_base+"/openvibe-modules/socket/trunc/include");
	incl.push_back(openvibe_base+"/openvibe-modules/stream/trunc/include");

	incl.push_back(openvibe_base+"/dependencies/boost/include");
	incl.push_back(openvibe_base+"/dependencies/openal/include");

	incl.push_back(openvibe_base+"/openvibe/trunc/include/openvibe");
    incl.push_back(openvibe_base+"/openvibe-toolkit/trunc/include/openvibe-toolkit");

	string result="";
	for(vector<string>::size_type i=0;i<incl.size();i++)
	{
		result+=incl[i]+";";
	}
	return result;
}

string openvibe_executable(string full_path)
{
	 int n =  full_path.find_last_of("/");

	 string executable;
	 executable.assign(full_path,n+1,full_path.size()-n); 
	 executable = custom_replace(executable,".vcxproj","");
	 return executable + ".exe";
}

string openvibe_base_folder(string full_path)
{
	 int n =  full_path.find_last_of("/");
	 string folder;
	 
	 folder.assign(full_path,0,n+1); 
	 folder = folder+"../../..";
	 return folder;
}

bool file_exists(const char *filename)
{
    ifstream ifile(filename);
    return ifile;
}

void create_vs_user_file(string path)
{
	 cout<<"Creating a new file:"+path<<endl;
	 ofstream myfile;
	 myfile.open (path.c_str());
     myfile << "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
	 myfile << "<Project xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">\r\n";
	 myfile << "</Project>\r\n";
     myfile.close();
}

int _tmain(int argc, TCHAR* argv[])
{
	cout<<"Please close ALL instances of Visual Studio and Openvibe!"<<endl;

	string project = "D:/Data/current_work/OpenVibe_src/local-tmp/visual/trunc/OpenViBE-acquisition-server-dynamic.vcxproj";
	if (argc>1) project = string(argv[1]);
	else 
	{
		cout<<"Usage: AdjustOpenVibe.exe \"D:\\Data\\current_work\\OpenVibe_src\\local-tmp\\visual\\trunc\\OpenViBE-acquisition-server-dynamic.vcxproj\""<<endl;
		return 0;
	}
	project = custom_replace(project,"\\","/");
	cout<<project<<endl;

	string openvibe_base = openvibe_base_folder(project);

    string executable = openvibe_executable(project);
	cout<<"Executable="<<executable<<endl;

	//1. Set Additional Include folders
	cout<<"1. Setting Additional Include Folders"<<endl;
	
	pugi::xml_document doc;

	pugi::xml_parse_result result = doc.load_file(project.c_str());
	cout<<result.description()<<endl;

	string includes = ";"+GetGlobalIncludes(openvibe_base);
    simple_walker walker("AdditionalIncludeDirectories",includes,true);
    doc.traverse(walker);

	cout<<"2. Setting Additional Libraries Folders"<<endl;
	string libs = ";"+openvibe_base+"/dependencies/boost/lib";
    walker = simple_walker("AdditionalLibraryDirectories",libs,true);
    doc.traverse(walker);

	//remove TARGET_HAS_ThirdPartyEmotivAPI
	cout<<"Emotiv driver disabled!"<<endl;
    replace_value_walker replace_walker = replace_value_walker("PreprocessorDefinitions","TARGET_HAS_ThirdPartyEmotivAPI;","");
    doc.traverse(replace_walker);

	doc.save_file(project.c_str());
	cout<<"Done."<<endl;
	
	//3. Change output directory
	cout<<"3. Set output folder to dist folder"<<endl;
	walker = simple_walker("OutDir","..\\..\\..\\dist\\bin",false);
    doc.traverse(walker);
	
	doc.save_file(project.c_str());
	cout<<"Done."<<endl;

	//4. Open .user file and set variables
	cout<<"4. Set environment variables."<<endl;

	string project_user = project + string(".user");

	if (!file_exists(project_user.c_str()))
	{
		create_vs_user_file(project_user);
	}

	result = doc.load_file(project_user.c_str());
	cout<<result.description()<<endl;
	//check if it is exe

	pugi::xml_node node = doc.child("Project");

	_chdir((openvibe_base + string("/scripts")).c_str());

	string s = openvibe_base + string("/scripts/win32-init_env_command.cmd>NUL & set");
	string with_openvibe_vars=exec(strdup(s.c_str()));

	pugi::xml_node property_group1 = node.append_child("PropertyGroup");
	pugi::xml_node env = property_group1.append_child("LocalDebuggerEnvironment");
    env.append_child(pugi::node_pcdata).set_value(with_openvibe_vars.c_str());

	pugi::xml_node property_group2 = node.append_child("PropertyGroup");
	pugi::xml_node cmd = property_group2.append_child("LocalDebuggerCommand");
	cmd.append_child(pugi::node_pcdata).set_value((openvibe_base+"/dist/bin/"+executable).c_str());

	pugi::xml_node property_group3 = node.append_child("PropertyGroup");
	pugi::xml_node folder = property_group3.append_child("LocalDebuggerWorkingDirectory");
    folder.append_child(pugi::node_pcdata).set_value((openvibe_base+"/dist/bin/").c_str());
	
	doc.save_file(project_user.c_str());
	
	cout<<"Done."<<endl;

	cout<<"TODO:"<<endl;
	cout<<"1) You need to disable the projects you do not need to rebuild in VS->Solution or Project properties->Confugration Manager"<<endl;
	cout<<"2) Set-up your start-up prject"<<endl;

	return 0;
}
