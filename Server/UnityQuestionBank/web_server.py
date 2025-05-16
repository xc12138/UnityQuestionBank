import os
import json
import random
from urllib.parse import unquote
import tornado.ioloop
import tornado.web
from tornado.web import RequestHandler

all_questions = {}

class BaseHandler(tornado.web.RequestHandler):
    # 解决跨域访问问题
    def set_default_headers(self):
        self.set_header("Access-Control-Allow-Origin", "*")   # 这个地方可以写域名
        self.set_header("Access-Control-Allow-Headers", "x-requested-with")
        self.set_header('Access-Control-Allow-Methods', 'POST, GET, OPTIONS')

    def post(self):
        self.write('some post')

    def get(self):
        self.write('some get')

    def options(self):
        # no body
        self.set_status(204)
        self.finish()

# 请求所有的题目类型
class get_question_types(BaseHandler):
    def get(self):
        question_types = []
        for k in all_questions.keys():
            question_types.append(k)
        self.write(json.dumps(question_types, ensure_ascii=False))

# 随机获取一道题目
class get_one_question(BaseHandler):
    def get(self):
        question_type = self.get_argument('question_type', default = 'C#基础')
        if not question_type in all_questions.keys():
            self.write('{ "error_code" : 1 }')
            return
        questions = all_questions[question_type]
        if 0 == len(questions):
            self.write('{ "error_code" : 1 }')
            return
        index = random.randint(0, len(questions) - 1)
        question = questions[index]
        question_txt = json.dumps(question, ensure_ascii=False)
        self.write('{ "error_code" : 0, "data" : %s }'%question_txt)

# 录入一道题目
class add_one_question(BaseHandler):
    def post(self):
        global all_questions
        question_info = {}
        question_type = unquote(self.get_argument('question_type'))
        question_info['question'] = unquote(self.get_argument('question'))
        question_info['code'] = unquote(self.get_argument('code'))
        question_info['answer'] = unquote(self.get_argument('answer'))
        all_questions[question_type].append(question_info)
        f = open('./question_bank/' + question_type + '.json', 'w', encoding='utf-8')
        f.write(json.dumps(all_questions[question_type], ensure_ascii=False, sort_keys=False, indent=2))
        f.close()
        self.write('{ "error_code" : 0 }')

# 创建tornado web服务器对象
def make_app():
    return tornado.web.Application([
        (r"/get_question_types", get_question_types),
        (r"/get_one_question", get_one_question),
        (r"/add_one_question", add_one_question),
    ])

# 读取所有的题目
def read_all_questions():
    global all_questions
    for root, dir, fs in os.walk('question_bank'):
        for f in fs:
            if f.endswith('.json'):
                fpath = os.path.join(root, f)
                fr = open(fpath, 'r', encoding='utf-8')
                txt = fr.read()
                if '' == txt:
                    txt = '[]'
                fr.close()
                f = f.replace('.json', '')
                all_questions[f] = json.loads(txt)
    
    print('read_all_questions ok')

if __name__ == "__main__":
    read_all_questions()
    app = make_app()
    app.listen(3209)
    print('localhost: 3209')
    tornado.ioloop.IOLoop.current().start()
